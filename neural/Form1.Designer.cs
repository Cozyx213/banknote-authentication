namespace neural
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblBanknote = new System.Windows.Forms.Label();
            this.txtHidden = new System.Windows.Forms.TextBox();
            this.txtLearningRate = new System.Windows.Forms.TextBox();
            this.txtEpochs = new System.Windows.Forms.TextBox();
            this.btnTuneBanknote = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpNN = new System.Windows.Forms.GroupBox();
            this.btnCreateNN = new System.Windows.Forms.Button();
            this.btnTrainNN = new System.Windows.Forms.Button();
            this.lblNNStatus = new System.Windows.Forms.Label();
            this.lblVariance = new System.Windows.Forms.Label();
            this.txtVariance = new System.Windows.Forms.TextBox();
            this.lblSkewness = new System.Windows.Forms.Label();
            this.txtSkewness = new System.Windows.Forms.TextBox();
            this.lblCurtosis = new System.Windows.Forms.Label();
            this.txtCurtosis = new System.Windows.Forms.TextBox();
            this.lblEntropy = new System.Windows.Forms.Label();
            this.txtEntropy = new System.Windows.Forms.TextBox();
            this.btnRunNN = new System.Windows.Forms.Button();
            this.lblOutput = new System.Windows.Forms.Label();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.tabCharts = new System.Windows.Forms.TabControl();
            this.tabLoss = new System.Windows.Forms.TabPage();
            this.chartLoss = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabMetrics = new System.Windows.Forms.TabPage();
            this.chartMetrics = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabConfusion = new System.Windows.Forms.TabPage();
            this.pnlConfusion = new System.Windows.Forms.Panel();
            this.tabRoc = new System.Windows.Forms.TabPage();
            this.chartRoc = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.grpNN.SuspendLayout();
            this.tabCharts.SuspendLayout();
            this.tabLoss.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartLoss)).BeginInit();
            this.tabMetrics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMetrics)).BeginInit();
            this.tabConfusion.SuspendLayout();
            this.tabRoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRoc)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblBanknote
            // 
            this.lblBanknote.AutoSize = true;
            this.lblBanknote.Location = new System.Drawing.Point(15, 68);
            this.lblBanknote.Name = "lblBanknote";
            this.lblBanknote.Size = new System.Drawing.Size(308, 13);
            this.lblBanknote.TabIndex = 12;
            this.lblBanknote.Text = "Banknote classifier:   hidden neurons  /  learning rate  /  epochs";
            // 
            // txtHidden
            // 
            this.txtHidden.Location = new System.Drawing.Point(13, 84);
            this.txtHidden.Name = "txtHidden";
            this.txtHidden.Size = new System.Drawing.Size(60, 20);
            this.txtHidden.TabIndex = 13;
            this.txtHidden.Text = "4";
            this.txtHidden.TextChanged += new System.EventHandler(this.txtHidden_TextChanged);
            // 
            // txtLearningRate
            // 
            this.txtLearningRate.Location = new System.Drawing.Point(120, 84);
            this.txtLearningRate.Name = "txtLearningRate";
            this.txtLearningRate.Size = new System.Drawing.Size(60, 20);
            this.txtLearningRate.TabIndex = 14;
            this.txtLearningRate.Text = "0.5";
            this.txtLearningRate.TextChanged += new System.EventHandler(this.txtLearningRate_TextChanged);
            // 
            // txtEpochs
            // 
            this.txtEpochs.Location = new System.Drawing.Point(225, 84);
            this.txtEpochs.Name = "txtEpochs";
            this.txtEpochs.Size = new System.Drawing.Size(60, 20);
            this.txtEpochs.TabIndex = 15;
            this.txtEpochs.Text = "250";
            this.txtEpochs.TextChanged += new System.EventHandler(this.txtEpochs_TextChanged);
            // 
            // btnTuneBanknote
            // 
            this.btnTuneBanknote.Location = new System.Drawing.Point(424, 34);
            this.btnTuneBanknote.Name = "btnTuneBanknote";
            this.btnTuneBanknote.Size = new System.Drawing.Size(263, 53);
            this.btnTuneBanknote.TabIndex = 17;
            this.btnTuneBanknote.Text = "Tune Hyperparameters";
            this.btnTuneBanknote.UseVisualStyleBackColor = true;
            this.btnTuneBanknote.Click += new System.EventHandler(this.btnTuneBanknote_Click);
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(65, 116);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(692, 525);
            this.txtLog.TabIndex = 18;
            this.txtLog.WordWrap = false;
            // 
            // grpNN
            // 
            this.grpNN.Controls.Add(this.btnCreateNN);
            this.grpNN.Controls.Add(this.btnTrainNN);
            this.grpNN.Controls.Add(this.lblNNStatus);
            this.grpNN.Controls.Add(this.lblVariance);
            this.grpNN.Controls.Add(this.txtVariance);
            this.grpNN.Controls.Add(this.lblBanknote);
            this.grpNN.Controls.Add(this.txtEpochs);
            this.grpNN.Controls.Add(this.txtLearningRate);
            this.grpNN.Controls.Add(this.lblSkewness);
            this.grpNN.Controls.Add(this.txtHidden);
            this.grpNN.Controls.Add(this.txtSkewness);
            this.grpNN.Controls.Add(this.lblCurtosis);
            this.grpNN.Controls.Add(this.txtCurtosis);
            this.grpNN.Controls.Add(this.lblEntropy);
            this.grpNN.Controls.Add(this.txtEntropy);
            this.grpNN.Controls.Add(this.btnRunNN);
            this.grpNN.Controls.Add(this.lblOutput);
            this.grpNN.Location = new System.Drawing.Point(763, 9);
            this.grpNN.Name = "grpNN";
            this.grpNN.Size = new System.Drawing.Size(507, 312);
            this.grpNN.TabIndex = 19;
            this.grpNN.TabStop = false;
            this.grpNN.Text = "Step-by-step NN (uses hidden / learning rate / epochs from the left)";
            // 
            // btnCreateNN
            // 
            this.btnCreateNN.Location = new System.Drawing.Point(15, 25);
            this.btnCreateNN.Name = "btnCreateNN";
            this.btnCreateNN.Size = new System.Drawing.Size(130, 40);
            this.btnCreateNN.TabIndex = 0;
            this.btnCreateNN.Text = "Create NN";
            this.btnCreateNN.UseVisualStyleBackColor = true;
            this.btnCreateNN.Click += new System.EventHandler(this.btnCreateNN_Click);
            // 
            // btnTrainNN
            // 
            this.btnTrainNN.Location = new System.Drawing.Point(155, 25);
            this.btnTrainNN.Name = "btnTrainNN";
            this.btnTrainNN.Size = new System.Drawing.Size(130, 40);
            this.btnTrainNN.TabIndex = 1;
            this.btnTrainNN.Text = "Train NN";
            this.btnTrainNN.UseVisualStyleBackColor = true;
            this.btnTrainNN.Click += new System.EventHandler(this.btnTrainNN_Click);
            // 
            // lblNNStatus
            // 
            this.lblNNStatus.AutoSize = true;
            this.lblNNStatus.Location = new System.Drawing.Point(291, 39);
            this.lblNNStatus.Name = "lblNNStatus";
            this.lblNNStatus.Size = new System.Drawing.Size(63, 13);
            this.lblNNStatus.TabIndex = 2;
            this.lblNNStatus.Text = "Not created";
            // 
            // lblVariance
            // 
            this.lblVariance.AutoSize = true;
            this.lblVariance.Location = new System.Drawing.Point(15, 113);
            this.lblVariance.Name = "lblVariance";
            this.lblVariance.Size = new System.Drawing.Size(49, 13);
            this.lblVariance.TabIndex = 3;
            this.lblVariance.Text = "Variance";
            // 
            // txtVariance
            // 
            this.txtVariance.Location = new System.Drawing.Point(90, 110);
            this.txtVariance.Name = "txtVariance";
            this.txtVariance.Size = new System.Drawing.Size(120, 20);
            this.txtVariance.TabIndex = 2;
            this.txtVariance.Text = "3.6216";
            // 
            // lblSkewness
            // 
            this.lblSkewness.AutoSize = true;
            this.lblSkewness.Location = new System.Drawing.Point(15, 143);
            this.lblSkewness.Name = "lblSkewness";
            this.lblSkewness.Size = new System.Drawing.Size(56, 13);
            this.lblSkewness.TabIndex = 4;
            this.lblSkewness.Text = "Skewness";
            // 
            // txtSkewness
            // 
            this.txtSkewness.Location = new System.Drawing.Point(90, 140);
            this.txtSkewness.Name = "txtSkewness";
            this.txtSkewness.Size = new System.Drawing.Size(120, 20);
            this.txtSkewness.TabIndex = 3;
            this.txtSkewness.Text = "8.6661";
            // 
            // lblCurtosis
            // 
            this.lblCurtosis.AutoSize = true;
            this.lblCurtosis.Location = new System.Drawing.Point(15, 173);
            this.lblCurtosis.Name = "lblCurtosis";
            this.lblCurtosis.Size = new System.Drawing.Size(44, 13);
            this.lblCurtosis.TabIndex = 5;
            this.lblCurtosis.Text = "Curtosis";
            // 
            // txtCurtosis
            // 
            this.txtCurtosis.Location = new System.Drawing.Point(90, 170);
            this.txtCurtosis.Name = "txtCurtosis";
            this.txtCurtosis.Size = new System.Drawing.Size(120, 20);
            this.txtCurtosis.TabIndex = 4;
            this.txtCurtosis.Text = "-2.8073";
            // 
            // lblEntropy
            // 
            this.lblEntropy.AutoSize = true;
            this.lblEntropy.Location = new System.Drawing.Point(15, 203);
            this.lblEntropy.Name = "lblEntropy";
            this.lblEntropy.Size = new System.Drawing.Size(43, 13);
            this.lblEntropy.TabIndex = 6;
            this.lblEntropy.Text = "Entropy";
            // 
            // txtEntropy
            // 
            this.txtEntropy.Location = new System.Drawing.Point(90, 200);
            this.txtEntropy.Name = "txtEntropy";
            this.txtEntropy.Size = new System.Drawing.Size(120, 20);
            this.txtEntropy.TabIndex = 5;
            this.txtEntropy.Text = "-0.44699";
            // 
            // btnRunNN
            // 
            this.btnRunNN.Location = new System.Drawing.Point(240, 110);
            this.btnRunNN.Name = "btnRunNN";
            this.btnRunNN.Size = new System.Drawing.Size(150, 110);
            this.btnRunNN.TabIndex = 6;
            this.btnRunNN.Text = "Run NN";
            this.btnRunNN.UseVisualStyleBackColor = true;
            this.btnRunNN.Click += new System.EventHandler(this.btnRunNN_Click);
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblOutput.Location = new System.Drawing.Point(15, 245);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(90, 19);
            this.lblOutput.TabIndex = 7;
            this.lblOutput.Text = "Output: -";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(65, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(263, 53);
            this.button1.TabIndex = 20;
            this.button1.Text = "Load Dataset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // tabCharts
            // 
            this.tabCharts.Controls.Add(this.tabLoss);
            this.tabCharts.Controls.Add(this.tabMetrics);
            this.tabCharts.Controls.Add(this.tabConfusion);
            this.tabCharts.Controls.Add(this.tabRoc);
            this.tabCharts.Location = new System.Drawing.Point(763, 305);
            this.tabCharts.Name = "tabCharts";
            this.tabCharts.SelectedIndex = 0;
            this.tabCharts.Size = new System.Drawing.Size(507, 340);
            this.tabCharts.TabIndex = 21;
            // 
            // tabLoss
            // 
            this.tabLoss.Controls.Add(this.chartLoss);
            this.tabLoss.Location = new System.Drawing.Point(4, 22);
            this.tabLoss.Name = "tabLoss";
            this.tabLoss.Padding = new System.Windows.Forms.Padding(3);
            this.tabLoss.Size = new System.Drawing.Size(499, 314);
            this.tabLoss.TabIndex = 0;
            this.tabLoss.Text = "Loss curve";
            this.tabLoss.UseVisualStyleBackColor = true;
            // 
            // chartLoss
            // 
            chartArea1.Name = "ChartArea1";
            this.chartLoss.ChartAreas.Add(chartArea1);
            this.chartLoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartLoss.Location = new System.Drawing.Point(3, 3);
            this.chartLoss.Name = "chartLoss";
            this.chartLoss.Size = new System.Drawing.Size(493, 308);
            this.chartLoss.TabIndex = 0;
            // 
            // tabMetrics
            // 
            this.tabMetrics.Controls.Add(this.chartMetrics);
            this.tabMetrics.Location = new System.Drawing.Point(4, 22);
            this.tabMetrics.Name = "tabMetrics";
            this.tabMetrics.Padding = new System.Windows.Forms.Padding(3);
            this.tabMetrics.Size = new System.Drawing.Size(432, 314);
            this.tabMetrics.TabIndex = 1;
            this.tabMetrics.Text = "Metrics";
            this.tabMetrics.UseVisualStyleBackColor = true;
            // 
            // chartMetrics
            // 
            chartArea2.Name = "ChartArea1";
            this.chartMetrics.ChartAreas.Add(chartArea2);
            this.chartMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMetrics.Location = new System.Drawing.Point(3, 3);
            this.chartMetrics.Name = "chartMetrics";
            this.chartMetrics.Size = new System.Drawing.Size(426, 308);
            this.chartMetrics.TabIndex = 0;
            // 
            // tabConfusion
            // 
            this.tabConfusion.Controls.Add(this.pnlConfusion);
            this.tabConfusion.Location = new System.Drawing.Point(4, 22);
            this.tabConfusion.Name = "tabConfusion";
            this.tabConfusion.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfusion.Size = new System.Drawing.Size(432, 314);
            this.tabConfusion.TabIndex = 2;
            this.tabConfusion.Text = "Confusion matrix";
            this.tabConfusion.UseVisualStyleBackColor = true;
            // 
            // pnlConfusion
            // 
            this.pnlConfusion.BackColor = System.Drawing.Color.White;
            this.pnlConfusion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConfusion.Location = new System.Drawing.Point(3, 3);
            this.pnlConfusion.Name = "pnlConfusion";
            this.pnlConfusion.Size = new System.Drawing.Size(426, 308);
            this.pnlConfusion.TabIndex = 0;
            // 
            // tabRoc
            // 
            this.tabRoc.Controls.Add(this.chartRoc);
            this.tabRoc.Location = new System.Drawing.Point(4, 22);
            this.tabRoc.Name = "tabRoc";
            this.tabRoc.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoc.Size = new System.Drawing.Size(432, 314);
            this.tabRoc.TabIndex = 3;
            this.tabRoc.Text = "ROC curve";
            this.tabRoc.UseVisualStyleBackColor = true;
            // 
            // chartRoc
            // 
            chartArea3.Name = "ChartArea1";
            this.chartRoc.ChartAreas.Add(chartArea3);
            this.chartRoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartRoc.Location = new System.Drawing.Point(3, 3);
            this.chartRoc.Name = "chartRoc";
            this.chartRoc.Size = new System.Drawing.Size(426, 308);
            this.chartRoc.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 653);
            this.Controls.Add(this.tabCharts);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.grpNN);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnTuneBanknote);
            this.Name = "Form1";
            this.Text = "Form1";
            this.grpNN.ResumeLayout(false);
            this.grpNN.PerformLayout();
            this.tabCharts.ResumeLayout(false);
            this.tabLoss.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartLoss)).EndInit();
            this.tabMetrics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMetrics)).EndInit();
            this.tabConfusion.ResumeLayout(false);
            this.tabRoc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartRoc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblBanknote;
        private System.Windows.Forms.TextBox txtHidden;
        private System.Windows.Forms.TextBox txtLearningRate;
        private System.Windows.Forms.TextBox txtEpochs;
        private System.Windows.Forms.Button btnTuneBanknote;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox grpNN;
        private System.Windows.Forms.Button btnCreateNN;
        private System.Windows.Forms.Button btnTrainNN;
        private System.Windows.Forms.Label lblNNStatus;
        private System.Windows.Forms.Label lblVariance;
        private System.Windows.Forms.Label lblSkewness;
        private System.Windows.Forms.Label lblCurtosis;
        private System.Windows.Forms.Label lblEntropy;
        private System.Windows.Forms.TextBox txtVariance;
        private System.Windows.Forms.TextBox txtSkewness;
        private System.Windows.Forms.TextBox txtCurtosis;
        private System.Windows.Forms.TextBox txtEntropy;
        private System.Windows.Forms.Button btnRunNN;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabCharts;
        private System.Windows.Forms.TabPage tabLoss;
        private System.Windows.Forms.TabPage tabMetrics;
        private System.Windows.Forms.TabPage tabConfusion;
        private System.Windows.Forms.TabPage tabRoc;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLoss;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMetrics;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRoc;
        private System.Windows.Forms.Panel pnlConfusion;
    }
}

