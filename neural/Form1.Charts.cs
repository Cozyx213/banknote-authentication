using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace neural
{
    /// <summary>
    /// The evaluation graphs under the Create / Train panel:
    ///   Loss curve        - train and test MSE per epoch (log scale)
    ///   Metrics           - accuracy / precision / recall / F1, train vs test
    ///   Confusion matrix  - TN / FP / FN / TP for train and test
    ///   ROC curve         - test set, every threshold, with AUC
    /// They are filled by UpdateCharts() after "Train NN" and cleared by ClearCharts() on "Create NN".
    /// </summary>
    public partial class Form1
    {
        private static readonly Color TrainColor = Color.FromArgb(46, 111, 191);  // blue
        private static readonly Color TestColor = Color.FromArgb(224, 123, 31);   // orange
        private static readonly Color CorrectColor = Color.FromArgb(46, 139, 87); // green
        private static readonly Color WrongColor = Color.FromArgb(200, 60, 60);   // red

        Banknote.Metrics bnTrainMetrics, bnTestMetrics; // results shown in the Metrics / Confusion tabs

        /// <summary>Sets up axes, series and titles once (called from the constructor).</summary>
        private void SetupCharts()
        {
            // ---- Loss curve ----
            ChartArea lossArea = chartLoss.ChartAreas[0];
            lossArea.AxisX.Title = "Epoch";
            lossArea.AxisY.Title = "MSE (log scale)";
            lossArea.AxisY.IsLogarithmic = true;
            lossArea.AxisY.LabelStyle.Format = "0.#####";
            StyleArea(lossArea);
            chartLoss.Titles.Add("Loss per epoch");
            chartLoss.Legends.Add(new Legend { Docking = Docking.Top, Alignment = StringAlignment.Center });
            chartLoss.Series.Add(NewSeries("Train MSE", SeriesChartType.Line, TrainColor));
            chartLoss.Series.Add(NewSeries("Test MSE", SeriesChartType.Line, TestColor));

            // ---- Metrics bars (horizontal, so the % labels have room next to each bar) ----
            ChartArea metricArea = chartMetrics.ChartAreas[0];
            metricArea.AxisY.Minimum = 0;
            metricArea.AxisY.Maximum = 1.18; // room for the value labels next to 100% bars (axis labels stop at 100%)
            metricArea.AxisY.Interval = 0.2;
            metricArea.AxisY.LabelStyle.Format = "0%";
            metricArea.AxisX.MajorGrid.Enabled = false;
            metricArea.AxisX.IsReversed = true; // Accuracy at the top
            StyleArea(metricArea);
            chartMetrics.Titles.Add("Classification metrics (threshold 0.5, positive = forged)");
            chartMetrics.Legends.Add(new Legend { Docking = Docking.Top, Alignment = StringAlignment.Center });
            foreach (var s in new[] { NewSeries("Train", SeriesChartType.Bar, TrainColor),
                                      NewSeries("Test", SeriesChartType.Bar, TestColor) })
            {
                s.IsValueShownAsLabel = true;
                s.LabelFormat = "0.0%";
                s["PointWidth"] = "0.8";
                chartMetrics.Series.Add(s);
            }

            // ---- ROC curve ----
            ChartArea rocArea = chartRoc.ChartAreas[0];
            rocArea.AxisX.Minimum = 0; rocArea.AxisX.Maximum = 1; rocArea.AxisX.Interval = 0.2;
            rocArea.AxisY.Minimum = 0; rocArea.AxisY.Maximum = 1; rocArea.AxisY.Interval = 0.2;
            rocArea.AxisX.Title = "False positive rate (genuine flagged as forged)";
            rocArea.AxisY.Title = "True positive rate (forged caught)";
            rocArea.AxisX.LabelStyle.Format = "0.0";
            rocArea.AxisY.LabelStyle.Format = "0.0";
            StyleArea(rocArea);
            chartRoc.Titles.Add("ROC curve (test set)");
            chartRoc.Legends.Add(new Legend { Docking = Docking.Top, Alignment = StringAlignment.Center });
            chartRoc.Series.Add(NewSeries("Network", SeriesChartType.Line, TestColor));
            Series chance = NewSeries("Random guess", SeriesChartType.Line, Color.Gray);
            chance.BorderDashStyle = ChartDashStyle.Dash;
            chance.BorderWidth = 1;
            chance.Points.AddXY(0, 0);
            chance.Points.AddXY(1, 1);
            chartRoc.Series.Add(chance);

            // ---- Confusion matrix (drawn by hand) ----
            pnlConfusion.Paint += pnlConfusion_Paint;
            pnlConfusion.Resize += (s, e) => pnlConfusion.Invalidate();
        }

        private static Series NewSeries(string name, SeriesChartType type, Color color)
        {
            return new Series(name) { ChartType = type, Color = color, BorderWidth = 2 };
        }

        private static void StyleArea(ChartArea area)
        {
            area.AxisX.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisX.TitleForeColor = Color.DimGray;
            area.AxisY.TitleForeColor = Color.DimGray;
        }

        /// <summary>Empties every graph (a new, untrained network has nothing to show yet).</summary>
        private void ClearCharts()
        {
            foreach (Series s in chartLoss.Series) s.Points.Clear();
            foreach (Series s in chartMetrics.Series) s.Points.Clear();
            chartRoc.Series["Network"].Points.Clear();
            chartRoc.Titles[0].Text = "ROC curve (test set)";
            bnTrainMetrics = bnTestMetrics = null;
            pnlConfusion.Invalidate();
        }

        /// <summary>Redraws every graph from the current network and the loss history.</summary>
        private void UpdateCharts()
        {
            // Loss curve: the whole history since Create NN. A log axis cannot show 0, so clamp tiny values.
            Series trainLoss = chartLoss.Series["Train MSE"], testLoss = chartLoss.Series["Test MSE"];
            trainLoss.Points.Clear();
            testLoss.Points.Clear();
            for (int i = 0; i < bnLoss.Count; i++) trainLoss.Points.AddXY(i + 1, Math.Max(bnLoss[i], 1e-10));
            for (int i = 0; i < bnTestLoss.Count; i++) testLoss.Points.AddXY(i + 1, Math.Max(bnTestLoss[i], 1e-10));

            // Metrics bars
            FillMetricBars(chartMetrics.Series["Train"], bnTrainMetrics);
            FillMetricBars(chartMetrics.Series["Test"], bnTestMetrics);

            // ROC curve on the test set
            List<double[]> roc = Banknote.RocCurve(banknoteNet, bnTest, out double auc);
            Series rocSeries = chartRoc.Series["Network"];
            rocSeries.Points.Clear();
            foreach (double[] p in roc) rocSeries.Points.AddXY(p[0], p[1]);
            chartRoc.Titles[0].Text = string.Format(Inv, "ROC curve (test set)   AUC = {0:0.0000}", auc);
            Log(string.Format(Inv, "Test ROC AUC = {0:0.0000}  (1.0 = perfect, 0.5 = random)", auc));

            pnlConfusion.Invalidate();
        }

        private static void FillMetricBars(Series series, Banknote.Metrics m)
        {
            series.Points.Clear();
            series.Points.AddXY("Accuracy", m.Accuracy);
            series.Points.AddXY("Precision", m.Precision);
            series.Points.AddXY("Recall", m.Recall);
            series.Points.AddXY("F1", m.F1);
        }

        // ---------------- Confusion matrix drawing ----------------

        private void pnlConfusion_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.White);

            int half = pnlConfusion.ClientSize.Width / 2;
            int height = pnlConfusion.ClientSize.Height;
            DrawConfusionMatrix(g, new Rectangle(0, 0, half, height), "Train set", bnTrainMetrics);
            DrawConfusionMatrix(g, new Rectangle(half, 0, half, height), "Test set", bnTestMetrics);
        }

        /// <summary>
        /// Draws one 2x2 matrix. Rows = actual class, columns = predicted class.
        /// Green cells are correct predictions, red cells are mistakes; darker = more notes.
        /// </summary>
        private void DrawConfusionMatrix(Graphics g, Rectangle area, string title, Banknote.Metrics m)
        {
            using (var titleFont = new Font("Segoe UI", 10f, FontStyle.Bold))
            using (var smallFont = new Font("Segoe UI", 8f))
            using (var countFont = new Font("Segoe UI", 14f, FontStyle.Bold))
            using (var center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var right = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(title, titleFont, Brushes.Black, new RectangleF(area.X, area.Y + 4, area.Width, 20), center);
                if (m == null)
                {
                    g.DrawString("Press \"Train NN\"\nto see this", smallFont, Brushes.Gray, area, center);
                    return;
                }

                const int labelWidth = 52;
                int cell = Math.Max(20, Math.Min((area.Width - labelWidth - 12) / 2, (area.Height - 120) / 2));
                int left = area.X + labelWidth;
                int top = area.Y + 66;

                // Column headers (predicted) and row headers (actual)
                g.DrawString("Predicted", smallFont, Brushes.DimGray, new RectangleF(left, top - 36, cell * 2, 16), center);
                g.DrawString("Genuine", smallFont, Brushes.Black, new RectangleF(left, top - 18, cell, 16), center);
                g.DrawString("Forged", smallFont, Brushes.Black, new RectangleF(left + cell, top - 18, cell, 16), center);
                g.DrawString("Actual", smallFont, Brushes.DimGray, new RectangleF(area.X, top - 18, labelWidth - 4, 16), right);
                g.DrawString("Genuine", smallFont, Brushes.Black, new RectangleF(area.X, top, labelWidth - 4, cell), right);
                g.DrawString("Forged", smallFont, Brushes.Black, new RectangleF(area.X, top + cell, labelWidth - 4, cell), right);

                int[,] counts = { { m.TN, m.FP }, { m.FN, m.TP } };
                string[,] names = { { "TN", "FP" }, { "FN", "TP" } };
                for (int row = 0; row < 2; row++)
                {
                    int rowTotal = counts[row, 0] + counts[row, 1];
                    for (int col = 0; col < 2; col++)
                    {
                        int count = counts[row, col];
                        double share = rowTotal == 0 ? 0 : (double)count / rowTotal; // % of that actual class
                        Color baseColor = row == col ? CorrectColor : WrongColor;
                        Color fill = count == 0 ? Color.White : Blend(Color.White, baseColor, 0.15 + 0.85 * share);
                        Brush textBrush = share > 0.5 ? Brushes.White : Brushes.Black;

                        var rect = new Rectangle(left + col * cell, top + row * cell, cell, cell);
                        using (var brush = new SolidBrush(fill)) g.FillRectangle(brush, rect);
                        g.DrawRectangle(Pens.Silver, rect);

                        g.DrawString(count.ToString(), countFont, textBrush,
                                     new RectangleF(rect.X, rect.Y, rect.Width, rect.Height - 14), center);
                        g.DrawString(string.Format(Inv, "{0} {1:0.0%}", names[row, col], share), smallFont, textBrush,
                                     new RectangleF(rect.X, rect.Bottom - 22, rect.Width, 16), center);
                    }
                }

                g.DrawString(string.Format(Inv, "{0} notes   accuracy {1:0.00%}", m.Count, m.Accuracy),
                             smallFont, Brushes.Black, new RectangleF(area.X, top + cell * 2 + 8, area.Width, 16), center);
            }
        }

        private static Color Blend(Color from, Color to, double t)
        {
            t = Math.Max(0, Math.Min(1, t));
            return Color.FromArgb((int)(from.R + (to.R - from.R) * t),
                                  (int)(from.G + (to.G - from.G) * t),
                                  (int)(from.B + (to.B - from.B) * t));
        }
    }
}
