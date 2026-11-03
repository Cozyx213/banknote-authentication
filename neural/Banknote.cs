using Backprop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace neural
{
    /// <summary>
    /// Banknote authentication (binary classification) using the  NeuralNet engine.
    /// Each row = 4 features (variance, skewness, curtosis, entropy) + label (0 = genuine, 1 = forged).
    /// Banknote Authentication dataset

    //    Where it came from: it's from the UCI Machine Learning Repository (University of California, Irvine).
    //    It was donated in 2012 by Volker Lohweg and Helene Dörksen of Ostwestfalen-Lippe University of Applied
    //    Sciences in Germany.

    //    How it was made: genuine and forged banknote-like specimens were photographed with an industrial print -
    //    inspection camera, as 400×400-pixel grayscale images at about 660 dpi.A wavelet transform was applied to
    //    each image, and 4 numbers were extracted from the result to describe its texture.

    //    What's in it: 1372 rows, 4 features and 1 label.

    /// </summary>
    public static class Banknote
    {
        /// <summary>Reads the CSV. Skips the header and any row that is blank or not fully numeric.</summary>
        public static List<double[]> LoadCsv(string path)
        {
            
            var rows = new List<double[]>();
            foreach (string line in File.ReadLines(path))
            {
                
                string[] cells = line.Split(',');
                var row = new double[5];
                bool ok = true;
                for (int i = 0; i < 5 && ok; i++)
                    ok = double.TryParse(cells[i], NumberStyles.Float, CultureInfo.InvariantCulture, out row[i]);

                if (ok) rows.Add(row);
            }
            return rows;
        }

        /// <summary>Shuffles the rows (fixed seed so results are repeatable) and splits off the test set.</summary>
      
        public static void Split(List<double[]> rows, double testRatio, out List<double[]> train, out List<double[]> test)
        {

            var rnd = new Random(42);
            List<double[]> shuffled = rows.OrderBy(r => rnd.Next()).ToList();
            int testCount = (int)(rows.Count * testRatio);
            test = shuffled.Take(testCount).ToList();
            train = shuffled.Skip(testCount).ToList();
        }

        /// <summary>
        /// MinMax-scales the 4 features to [0,1]: x' = (x - min) / (max - min).
        /// Min/max come from the TRAINING set only, so no test information leaks into training.
        /// Returns scaled copies; the label column is left unchanged.
        ///  izza Min-Max normalization:
        /// </summary>
        public static void MinMaxScale(List<double[]> train, List<double[]> test,
                                       out List<double[]> trainScaled, out List<double[]> testScaled)
        {
            MinMaxScale(train, test, out trainScaled, out testScaled, out _, out _);
        }

        /// <summary>Same as above, but also returns the min/max so new inputs can be scaled the same way.</summary>
        public static void MinMaxScale(List<double[]> train, List<double[]> test,
                                       out List<double[]> trainScaled, out List<double[]> testScaled,
                                       out double[] min, out double[] max)
        {
            double[] mn = new double[4];
            double[] mx = new double[4];

            for (int f = 0; f < 4; f++)
            {
                mn[f] = train.Min(r => r[f]);
                mx[f] = train.Max(r => r[f]);
            }
            trainScaled = train.Select(r => ScaleRow(r, mn, mx)).ToList();
            testScaled = test.Select(r => ScaleRow(r, mn, mx)).ToList();

            min = mn;
            max = mx;
        }

        /// <summary>Scales the first 4 values of a row with x' = (x - min) / (max - min). Other columns are copied as-is.</summary>
        public static double[] ScaleRow(double[] row, double[] min, double[] max)
        {
            var s = (double[])row.Clone();
            for (int f = 0; f < 4; f++)
                s[f] = (row[f] - min[f]) / (max[f] - min[f]);
            return s;
        }

        /// <summary>
        /// Trains a 4-hidden-1 network with per-sample backpropagation (NeuralNet.learn()).
        /// Returns the mean squared error of every epoch so the loss curve can be logged.
        /// </summary>
        public static NeuralNet Train(List<double[]> train, int hidden, double learningRate, int epochs,
                                      out List<double> lossPerEpoch)
        {
            var nn = new NeuralNet(4, hidden, 1, learningRate, learningRate);
            lossPerEpoch = TrainExisting(nn, train, epochs);
            return nn;
        }

        /// <summary>Trains an already-created network for more epochs. Returns the MSE of every epoch.</summary>
        public static List<double> TrainExisting(NeuralNet nn, List<double[]> train, int epochs)
        {
            return TrainExisting(nn, train, epochs, null, out _);
        }

        /// <summary>
        /// Same as above, but after every epoch also measures the MSE on the test set (no learning on it),
        /// so the train and test loss curves can be plotted together.
        /// </summary>
        public static List<double> TrainExisting(NeuralNet nn, List<double[]> train, int epochs,
                                                 List<double[]> test, out List<double> testLossPerEpoch)
        {
            List<double> lossPerEpoch;
            var rnd = new Random(1);
            lossPerEpoch = new List<double>();
            testLossPerEpoch = new List<double>();

            for (int epoch = 1; epoch <= epochs; epoch++)
            {
                double sumSquaredError = 0;
                foreach (double[] row in train.OrderBy(r => rnd.Next())) // new random order each epoch
                {
                    for (int i = 0; i < 4; i++) nn.setInputs(i, row[i]);
                    nn.setDesiredOutput(0, row[4]);
                    nn.learn();
                    double error = row[4] - nn.getOuputData(0); // output from learn()'s forward pass
                    sumSquaredError += error * error;
                }
                lossPerEpoch.Add(sumSquaredError / train.Count);
                if (test != null)
                    testLossPerEpoch.Add(ComputeMetrics(nn, test).Mse);
            }
            return lossPerEpoch;
        }

        public static double Predict(NeuralNet nn, double[] row)
        {
            for (int i = 0; i < 4; i++) nn.setInputs(i, row[i]);
            nn.run();
            return nn.getOuputData(0);
        }

        /// <summary>
        /// Results of testing a network on a data set. Positive class = forged (1), threshold 0.5.
        /// TP = forged predicted forged, TN = genuine predicted genuine,
        /// FP = genuine predicted forged, FN = forged predicted genuine.
        /// </summary>
        public class Metrics
        {
            public int TP, TN, FP, FN;
            public double Mse;

            public int Count { get { return TP + TN + FP + FN; } }
            /// <summary>Share of all notes classified correctly.</summary>
            public double Accuracy { get { return Count == 0 ? 0 : (double)(TP + TN) / Count; } }
            /// <summary>Of the notes predicted forged, how many really were forged.</summary>
            public double Precision { get { return TP + FP == 0 ? 0 : (double)TP / (TP + FP); } }
            /// <summary>Of the notes that really were forged, how many were caught.</summary>
            public double Recall { get { return TP + FN == 0 ? 0 : (double)TP / (TP + FN); } }
            /// <summary>Harmonic mean of precision and recall.</summary>
            public double F1 { get { return Precision + Recall == 0 ? 0 : 2 * Precision * Recall / (Precision + Recall); } }

            public string Report()
            {
                var sb = new StringBuilder();
                sb.AppendLine("Confusion matrix (rows = actual, cols = predicted):");
                sb.AppendLine("              genuine  forged");
                sb.AppendLine(string.Format("  genuine   {0,8} {1,7}", TN, FP));
                sb.AppendLine(string.Format("  forged    {0,8} {1,7}", FN, TP));
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "Accuracy {0:0.00%}  Precision {1:0.00%}  Recall {2:0.00%}  F1 {3:0.00%}  MSE {4:0.00000}",
                    Accuracy, Precision, Recall, F1, Mse));
                return sb.ToString();
            }
        }

        /// <summary>Runs the network on every row (no learning) and counts TP/TN/FP/FN and the MSE.</summary>
        public static Metrics ComputeMetrics(NeuralNet nn, List<double[]> data)
        {
            var m = new Metrics();
            double sse = 0;
            foreach (double[] row in data)
            {
                double output = Predict(nn, row);
                sse += (row[4] - output) * (row[4] - output);
                bool predictedForged = output >= 0.5, actuallyForged = row[4] == 1;
                if (predictedForged && actuallyForged) m.TP++;
                else if (predictedForged) m.FP++;
                else if (actuallyForged) m.FN++;
                else m.TN++;
            }
            m.Mse = data.Count == 0 ? 0 : sse / data.Count;
            return m;
        }

        /// <summary>Accuracy, precision, recall, F1 and MSE on a data set, as text. Positive class = forged (1).</summary>
        public static string Evaluate(NeuralNet nn, List<double[]> data, out double accuracy, out double mse)
        {
            Metrics m = ComputeMetrics(nn, data);
            accuracy = m.Accuracy;
            mse = m.Mse;
            return m.Report();
        }

        /// <summary>
        /// ROC curve: tries every possible threshold (not just 0.5) and records the
        /// false positive rate (x) and true positive rate (y) for each one.
        /// AUC = area under that curve (1.0 = perfect, 0.5 = random guessing).
        /// </summary>
        public static List<double[]> RocCurve(NeuralNet nn, List<double[]> data, out double auc)
        {
            var scored = data.Select(r => new { Score = Predict(nn, r), Forged = r[4] == 1 })
                             .OrderByDescending(s => s.Score)
                             .ToList();
            int positives = scored.Count(s => s.Forged);
            int negatives = scored.Count - positives;

            var points = new List<double[]> { new double[] { 0, 0 } };
            int tp = 0, fp = 0;
            auc = 0;
            int i = 0;
            while (i < scored.Count)
            {
                // Lower the threshold to the next score (rows with the same score move together).
                double score = scored[i].Score;
                while (i < scored.Count && scored[i].Score == score)
                {
                    if (scored[i].Forged) tp++; else fp++;
                    i++;
                }
                double fpr = negatives == 0 ? 0 : (double)fp / negatives;
                double tpr = positives == 0 ? 0 : (double)tp / positives;
                double[] prev = points[points.Count - 1];
                auc += (fpr - prev[0]) * (tpr + prev[1]) / 2; // trapezoid rule
                points.Add(new[] { fpr, tpr });
            }
            return points;
        }
    }
}
