using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Backprop;
namespace neural
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupCharts();
        }
        NeuralNet nn;
        Bitmap loaded;
        private void button1_Click(object sender, EventArgs e)
        {
            nn = new NeuralNet(2, 5, 1);
        }


        private string FileName;
        NeuralNet banknoteNet;          // network made by "Create NN"
        List<double[]> bnTrain, bnTest; // scaled data used by "Train NN"
        double[] bnMin, bnMax;          // scaling from the training set, reused for "Run NN" inputs
        int bnEpochsTrained;
        List<double> bnLoss = new List<double>();     // train loss of every epoch since "Create NN"
        List<double> bnTestLoss = new List<double>(); // test loss of every epoch since "Create NN"

        // ---------------- Banknote authentication ----------------

      

        /// <summary>Load CSV -> 80/20 train/test split -> MinMax scale (fit on train only).</summary>
        private void PrepareBanknoteData(out List<double[]> train, out List<double[]> test)
        {
            Log("Loading and Parsing CSV");
            
            List<double[]> rows = Banknote.LoadCsv(FileName);


            Log("Spliting Data with 20% being test");

            Banknote.Split(rows, 0.2, out List<double[]> rawTrain, out List<double[]> rawTest);

            Log("Min-Max Normalizing the data");

            Banknote.MinMaxScale(rawTrain, rawTest, out train, out test);
        }

        private void Log(string line)
        {
            txtLog.AppendText(line + Environment.NewLine);
        }

        private void btnTuneBanknote_Click(object sender, EventArgs e)
        {
            int[] hiddenSizes = { 2, 4, 8};
            double[] learningRates = { 0.01, 0.2, 0.5};
            int[] epochCounts = { 100, 250};

            if (FileName == null)
            {
                MessageBox.Show("Load Data first");
                return;
            }
            PrepareBanknoteData(out List<double[]> train, out List<double[]> test);
            Log("hidden    lr  epochs   final train MSE   test acc   test MSE");
            var csv = new StringBuilder("hidden,learning_rate,epochs,train_mse,test_accuracy,test_mse\r\n");

            Cursor = Cursors.WaitCursor;
            foreach (int hidden in hiddenSizes)
                foreach (double lr in learningRates)
                    foreach (int epochs in epochCounts)
                    {
                        NeuralNet net = Banknote.Train(train, hidden, lr, epochs, out List<double> loss);
                        Banknote.Evaluate(net, test, out double accuracy, out double mse);
                        string line = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                            "{0,6} {1,5} {2,7} {3,17:0.000000} {4,10:0.00%} {5,10:0.000000}",
                            hidden, lr, epochs, loss[loss.Count - 1], accuracy, mse);
                        Log(line);
                        csv.AppendLine(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                            "{0},{1},{2},{3},{4},{5}", hidden, lr, epochs, loss[loss.Count - 1], accuracy, mse));
                        Application.DoEvents(); // keep the log updating while the grid runs
                    }
            Cursor = Cursors.Default;

            string path = System.IO.Path.Combine(Application.StartupPath, "tuning_results.csv");
            System.IO.File.WriteAllText(path, csv.ToString());
            Log("Results saved to " + path);
        }

        private void lblBanknote_Click(object sender, EventArgs e)
        {

        }

        // ---------------- Create / Train / Run (step by step) ----------------


        private static readonly System.Globalization.CultureInfo Inv = System.Globalization.CultureInfo.InvariantCulture;

        private void btnCreateNN_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtHidden.Text, out int hidden) || hidden < 1 ||
                !double.TryParse(txtLearningRate.Text, System.Globalization.NumberStyles.Float, Inv, out double lr))
            {
                MessageBox.Show("Enter a valid hidden neuron count and learning rate.", "Create NN");
                return;
            }
            if (FileName == null)
            {
                MessageBox.Show("Load Data first");
                return;
            }
            Log("Loading and Parsing CSV");
            List<double[]> rows = Banknote.LoadCsv(FileName);

            Log("Spliting Data with 20% being test");
            Banknote.Split(rows, 0.2, out List<double[]> rawTrain, out List<double[]> rawTest);

            Log("Min-Max Normalizing the data");
            Banknote.MinMaxScale(rawTrain, rawTest, out bnTrain, out bnTest, out bnMin, out bnMax);

            Log("Creating NN: 4 inputs -> " + hidden + " hidden -> 1 output   (learning rate " + lr.ToString(Inv) + ")");
            banknoteNet = new NeuralNet(4, hidden, 1, lr, lr);

            bnEpochsTrained = 0;
            bnLoss.Clear();
            bnTestLoss.Clear();
            ClearCharts();


            Log("Train rows: " + bnTrain.Count + "   Test rows: " + bnTest.Count);
            Log("Network is untrained. Press \"Train NN\".");
            lblNNStatus.Text = "Created (untrained)";
            lblOutput.Text = "Output: -";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog2.FileName = "";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog2.FileName;
                Log("Loaded: " + FileName);
            }
        }

        private void txtHidden_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLearningRate_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEpochs_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTrainNN_Click(object sender, EventArgs e)
        {
            if (banknoteNet == null)
            {
                MessageBox.Show("Press \"Create NN\" first.", "Train NN");
                return;
            }
            if (!int.TryParse(txtEpochs.Text, out int epochs) || epochs < 1)
            {
                MessageBox.Show("Enter a valid number of epochs.", "Train NN");
                return;
            }

            Cursor = Cursors.WaitCursor;
            List<double> loss = Banknote.TrainExisting(banknoteNet, bnTrain, epochs, bnTest, out List<double> testLoss);
            Cursor = Cursors.Default;

            Log("");
            Log("Training " + epochs + " epochs (starting at epoch " + (bnEpochsTrained + 1) + ")...");
            for (int i = 0; i < loss.Count; i++)
            {
                int epoch = bnEpochsTrained + i + 1;
                if (i == 0 || epoch % 100 == 0 || i == loss.Count - 1)
                    Log(string.Format(Inv, "Epoch {0,5}   train MSE = {1:0.000000}   test MSE = {2:0.000000}",
                                      epoch, loss[i], testLoss[i]));
            }
            bnEpochsTrained += epochs;
            bnLoss.AddRange(loss);
            bnTestLoss.AddRange(testLoss);

            // Export the loss of every epoch (since Create NN) to CSV.
            var csv = new StringBuilder("epoch,train_mse,test_mse\r\n");
            for (int i = 0; i < bnLoss.Count; i++)
                csv.AppendLine((i + 1) + "," + bnLoss[i].ToString(Inv) + "," + bnTestLoss[i].ToString(Inv));
            string logPath = System.IO.Path.Combine(Application.StartupPath, "training_log.csv");
            System.IO.File.WriteAllText(logPath, csv.ToString());
            Log("Loss per epoch saved to " + logPath);

            Log("");
            bnTrainMetrics = Banknote.ComputeMetrics(banknoteNet, bnTrain);
            bnTestMetrics = Banknote.ComputeMetrics(banknoteNet, bnTest);
            Log("== TRAIN SET ==");
            Log(bnTrainMetrics.Report());
            Log("== TEST SET (never seen during training) ==");
            Log(bnTestMetrics.Report());
            UpdateCharts();
            lblNNStatus.Text = string.Format(Inv, "Trained {0} epochs, test acc {1:0.00%}", bnEpochsTrained, bnTestMetrics.Accuracy);
        }

        private void btnRunNN_Click(object sender, EventArgs e)
        {
            if (banknoteNet == null)
            {
                MessageBox.Show("Press \"Create NN\" (and \"Train NN\") first.", "Run NN");
                return;
            }

            TextBox[] boxes = { txtVariance, txtSkewness, txtCurtosis, txtEntropy };
            var raw = new double[4];
            for (int i = 0; i < 4; i++)
            {
                if (!double.TryParse(boxes[i].Text, System.Globalization.NumberStyles.Float, Inv, out raw[i]))
                {
                    MessageBox.Show("\"" + boxes[i].Text + "\" is not a number (use . as the decimal point).", "Run NN");
                    boxes[i].Focus();
                    return;
                }
            }

            double[] scaled = Banknote.ScaleRow(raw, bnMin, bnMax);
            double output = Banknote.Predict(banknoteNet, scaled);
            bool forged = output >= 0.5;

            lblOutput.Text = string.Format(Inv, "Output: {0:0.0000}  ->  {1}", output, forged ? "FORGED (1)" : "GENUINE (0)");
            lblOutput.ForeColor = forged ? Color.Firebrick : Color.ForestGreen;
            Log(string.Format(Inv, "Run [{0}, {1}, {2}, {3}] -> {4:0.0000} = {5}{6}",
                raw[0], raw[1], raw[2], raw[3], output, forged ? "forged" : "genuine",
                bnEpochsTrained == 0 ? "   (network is untrained!)" : ""));
        }
    }
}
