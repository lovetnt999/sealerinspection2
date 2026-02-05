namespace AvisSealer
{
    partial class SealerBeadMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel flpInputs;
        private System.Windows.Forms.FlowLayoutPanel flpOutputs;
        private System.Windows.Forms.Panel pnlTopButtons;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblInputHeader;
        private System.Windows.Forms.Label lblOutputHeader;
        private System.Windows.Forms.Label lblInput01;
        private System.Windows.Forms.Label lblInput02;
        private System.Windows.Forms.Label lblInput03;
        private System.Windows.Forms.Label lblInput04;
        private System.Windows.Forms.Label lblInput05;
        private System.Windows.Forms.Label lblInput06;
        private System.Windows.Forms.Label lblOutput01;
        private System.Windows.Forms.Label lblOutput02;
        private System.Windows.Forms.Label lblOutput03;
        private System.Windows.Forms.TableLayoutPanel tlpMiddle;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.RichTextBox rtbSystemLog;
        private System.Windows.Forms.Label lblDetailHeader;
        private System.Windows.Forms.TableLayoutPanel tlpDetail;
        private System.Windows.Forms.Label lblProductTitle;
        private System.Windows.Forms.Label lblEquipmentTitle;
        private System.Windows.Forms.Label lblProgressTitle;
        private System.Windows.Forms.Label lblSealerWidthTitle;
        private System.Windows.Forms.Label lblScoreTitle;
        private System.Windows.Forms.Label lblShortCircuitTitle;
        private System.Windows.Forms.Label lblProductValue;
        private System.Windows.Forms.Label lblEquipmentValue;
        private System.Windows.Forms.Label lblProgressValue;
        private System.Windows.Forms.Label lblSealerWidthValue;
        private System.Windows.Forms.Label lblScoreValue;
        private System.Windows.Forms.Label lblShortCircuitValue;
        private System.Windows.Forms.Panel pnlChart;
        private DonutChartPanel donutChart;
        private System.Windows.Forms.Label lblChartHeader;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private System.Windows.Forms.Panel pnlCam1;
        private System.Windows.Forms.Panel pnlCam2;
        private System.Windows.Forms.PictureBox pbCam1;
        private System.Windows.Forms.PictureBox pbCam2;
        private System.Windows.Forms.Label lblCam1Header;
        private System.Windows.Forms.Label lblCam2Header;
        private System.Windows.Forms.Label lblCam1Result;
        private System.Windows.Forms.Label lblCam2Result;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTopButtons = new System.Windows.Forms.Panel();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.flpOutputs = new System.Windows.Forms.FlowLayoutPanel();
            this.lblOutputHeader = new System.Windows.Forms.Label();
            this.lblOutput01 = new System.Windows.Forms.Label();
            this.lblOutput02 = new System.Windows.Forms.Label();
            this.lblOutput03 = new System.Windows.Forms.Label();
            this.flpInputs = new System.Windows.Forms.FlowLayoutPanel();
            this.lblInputHeader = new System.Windows.Forms.Label();
            this.lblInput01 = new System.Windows.Forms.Label();
            this.lblInput02 = new System.Windows.Forms.Label();
            this.lblInput03 = new System.Windows.Forms.Label();
            this.lblInput04 = new System.Windows.Forms.Label();
            this.lblInput05 = new System.Windows.Forms.Label();
            this.lblInput06 = new System.Windows.Forms.Label();
            this.tlpMiddle = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.lblDetailHeader = new System.Windows.Forms.Label();
            this.tlpDetail = new System.Windows.Forms.TableLayoutPanel();
            this.lblProductTitle = new System.Windows.Forms.Label();
            this.lblEquipmentTitle = new System.Windows.Forms.Label();
            this.lblProgressTitle = new System.Windows.Forms.Label();
            this.lblSealerWidthTitle = new System.Windows.Forms.Label();
            this.lblScoreTitle = new System.Windows.Forms.Label();
            this.lblShortCircuitTitle = new System.Windows.Forms.Label();
            this.lblProductValue = new System.Windows.Forms.Label();
            this.lblEquipmentValue = new System.Windows.Forms.Label();
            this.lblProgressValue = new System.Windows.Forms.Label();
            this.lblSealerWidthValue = new System.Windows.Forms.Label();
            this.lblScoreValue = new System.Windows.Forms.Label();
            this.lblShortCircuitValue = new System.Windows.Forms.Label();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.lblChartHeader = new System.Windows.Forms.Label();
            this.donutChart = new DonutChartPanel();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.rtbSystemLog = new System.Windows.Forms.RichTextBox();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCam1 = new System.Windows.Forms.Panel();
            this.lblCam1Header = new System.Windows.Forms.Label();
            this.pbCam1 = new System.Windows.Forms.PictureBox();
            this.lblCam1Result = new System.Windows.Forms.Label();
            this.pnlCam2 = new System.Windows.Forms.Panel();
            this.lblCam2Header = new System.Windows.Forms.Label();
            this.pbCam2 = new System.Windows.Forms.PictureBox();
            this.lblCam2Result = new System.Windows.Forms.Label();
            this.tlpRoot.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlTopButtons.SuspendLayout();
            this.flpOutputs.SuspendLayout();
            this.flpInputs.SuspendLayout();
            this.tlpMiddle.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            this.tlpDetail.SuspendLayout();
            this.pnlChart.SuspendLayout();
            this.pnlLog.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.pnlCam1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCam1)).BeginInit();
            this.pnlCam2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCam2)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRoot.Controls.Add(this.pnlTop, 0, 0);
            this.tlpRoot.Controls.Add(this.tlpMiddle, 0, 1);
            this.tlpRoot.Controls.Add(this.tlpBottom, 0, 2);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.RowCount = 3;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpRoot.Location = new System.Drawing.Point(0, 0);
            this.tlpRoot.Name = "tlpRoot";
            this.tlpRoot.Size = new System.Drawing.Size(1400, 900);
            this.tlpRoot.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.pnlTopButtons);
            this.pnlTop.Controls.Add(this.flpOutputs);
            this.pnlTop.Controls.Add(this.flpInputs);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1400, 70);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(356, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Sealer Bead Inspection System v1.0";
            // 
            // pnlTopButtons
            // 
            this.pnlTopButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTopButtons.Controls.Add(this.btnSetting);
            this.pnlTopButtons.Controls.Add(this.btnHistory);
            this.pnlTopButtons.Controls.Add(this.btnExit);
            this.pnlTopButtons.Location = new System.Drawing.Point(1230, 16);
            this.pnlTopButtons.Name = "pnlTopButtons";
            this.pnlTopButtons.Size = new System.Drawing.Size(150, 40);
            this.pnlTopButtons.TabIndex = 3;
            // 
            // btnSetting
            // 
            this.btnSetting.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSetting.ForeColor = System.Drawing.Color.White;
            this.btnSetting.Location = new System.Drawing.Point(0, 0);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(40, 40);
            this.btnSetting.TabIndex = 0;
            this.btnSetting.Text = "⚙";
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnHistory.ForeColor = System.Drawing.Color.White;
            this.btnHistory.Location = new System.Drawing.Point(50, 0);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(40, 40);
            this.btnHistory.TabIndex = 1;
            this.btnHistory.Text = "📊";
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(80, 20, 20);
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(100, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(40, 40);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // flpOutputs
            // 
            this.flpOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpOutputs.AutoSize = true;
            this.flpOutputs.Controls.Add(this.lblOutputHeader);
            this.flpOutputs.Controls.Add(this.lblOutput01);
            this.flpOutputs.Controls.Add(this.lblOutput02);
            this.flpOutputs.Controls.Add(this.lblOutput03);
            this.flpOutputs.Location = new System.Drawing.Point(1000, 10);
            this.flpOutputs.Name = "flpOutputs";
            this.flpOutputs.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.flpOutputs.Size = new System.Drawing.Size(200, 50);
            this.flpOutputs.TabIndex = 2;
            // 
            // lblOutputHeader
            // 
            this.lblOutputHeader.AutoSize = true;
            this.lblOutputHeader.ForeColor = System.Drawing.Color.White;
            this.lblOutputHeader.Location = new System.Drawing.Point(9, 6);
            this.lblOutputHeader.Name = "lblOutputHeader";
            this.lblOutputHeader.Size = new System.Drawing.Size(53, 20);
            this.lblOutputHeader.TabIndex = 0;
            this.lblOutputHeader.Text = "Output";
            // 
            // lblOutput01
            // 
            this.lblOutput01.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblOutput01.ForeColor = System.Drawing.Color.White;
            this.lblOutput01.Location = new System.Drawing.Point(68, 6);
            this.lblOutput01.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblOutput01.Name = "lblOutput01";
            this.lblOutput01.Size = new System.Drawing.Size(18, 18);
            this.lblOutput01.TabIndex = 1;
            this.lblOutput01.Text = "1";
            this.lblOutput01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutput02
            // 
            this.lblOutput02.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblOutput02.ForeColor = System.Drawing.Color.White;
            this.lblOutput02.Location = new System.Drawing.Point(92, 6);
            this.lblOutput02.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblOutput02.Name = "lblOutput02";
            this.lblOutput02.Size = new System.Drawing.Size(18, 18);
            this.lblOutput02.TabIndex = 2;
            this.lblOutput02.Text = "2";
            this.lblOutput02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutput03
            // 
            this.lblOutput03.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblOutput03.ForeColor = System.Drawing.Color.White;
            this.lblOutput03.Location = new System.Drawing.Point(116, 6);
            this.lblOutput03.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblOutput03.Name = "lblOutput03";
            this.lblOutput03.Size = new System.Drawing.Size(18, 18);
            this.lblOutput03.TabIndex = 3;
            this.lblOutput03.Text = "3";
            this.lblOutput03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flpInputs
            // 
            this.flpInputs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpInputs.AutoSize = true;
            this.flpInputs.Controls.Add(this.lblInputHeader);
            this.flpInputs.Controls.Add(this.lblInput01);
            this.flpInputs.Controls.Add(this.lblInput02);
            this.flpInputs.Controls.Add(this.lblInput03);
            this.flpInputs.Controls.Add(this.lblInput04);
            this.flpInputs.Controls.Add(this.lblInput05);
            this.flpInputs.Controls.Add(this.lblInput06);
            this.flpInputs.Location = new System.Drawing.Point(760, 10);
            this.flpInputs.Name = "flpInputs";
            this.flpInputs.Padding = new System.Windows.Forms.Padding(6);
            this.flpInputs.Size = new System.Drawing.Size(230, 50);
            this.flpInputs.TabIndex = 1;
            // 
            // lblInputHeader
            // 
            this.lblInputHeader.AutoSize = true;
            this.lblInputHeader.ForeColor = System.Drawing.Color.White;
            this.lblInputHeader.Location = new System.Drawing.Point(9, 6);
            this.lblInputHeader.Name = "lblInputHeader";
            this.lblInputHeader.Size = new System.Drawing.Size(38, 20);
            this.lblInputHeader.TabIndex = 0;
            this.lblInputHeader.Text = "Input";
            // 
            // lblInput01
            // 
            this.lblInput01.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblInput01.ForeColor = System.Drawing.Color.White;
            this.lblInput01.Location = new System.Drawing.Point(53, 6);
            this.lblInput01.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblInput01.Name = "lblInput01";
            this.lblInput01.Size = new System.Drawing.Size(18, 18);
            this.lblInput01.TabIndex = 1;
            this.lblInput01.Text = "1";
            this.lblInput01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInput02
            // 
            this.lblInput02.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblInput02.ForeColor = System.Drawing.Color.White;
            this.lblInput02.Location = new System.Drawing.Point(77, 6);
            this.lblInput02.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblInput02.Name = "lblInput02";
            this.lblInput02.Size = new System.Drawing.Size(18, 18);
            this.lblInput02.TabIndex = 2;
            this.lblInput02.Text = "2";
            this.lblInput02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInput03
            // 
            this.lblInput03.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblInput03.ForeColor = System.Drawing.Color.White;
            this.lblInput03.Location = new System.Drawing.Point(101, 6);
            this.lblInput03.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblInput03.Name = "lblInput03";
            this.lblInput03.Size = new System.Drawing.Size(18, 18);
            this.lblInput03.TabIndex = 3;
            this.lblInput03.Text = "3";
            this.lblInput03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInput04
            // 
            this.lblInput04.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblInput04.ForeColor = System.Drawing.Color.White;
            this.lblInput04.Location = new System.Drawing.Point(125, 6);
            this.lblInput04.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblInput04.Name = "lblInput04";
            this.lblInput04.Size = new System.Drawing.Size(18, 18);
            this.lblInput04.TabIndex = 4;
            this.lblInput04.Text = "4";
            this.lblInput04.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInput05
            // 
            this.lblInput05.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblInput05.ForeColor = System.Drawing.Color.White;
            this.lblInput05.Location = new System.Drawing.Point(149, 6);
            this.lblInput05.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblInput05.Name = "lblInput05";
            this.lblInput05.Size = new System.Drawing.Size(18, 18);
            this.lblInput05.TabIndex = 5;
            this.lblInput05.Text = "5";
            this.lblInput05.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInput06
            // 
            this.lblInput06.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblInput06.ForeColor = System.Drawing.Color.White;
            this.lblInput06.Location = new System.Drawing.Point(173, 6);
            this.lblInput06.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblInput06.Name = "lblInput06";
            this.lblInput06.Size = new System.Drawing.Size(18, 18);
            this.lblInput06.TabIndex = 6;
            this.lblInput06.Text = "6";
            this.lblInput06.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpMiddle
            // 
            this.tlpMiddle.ColumnCount = 3;
            this.tlpMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpMiddle.Controls.Add(this.pnlDetail, 0, 0);
            this.tlpMiddle.Controls.Add(this.pnlChart, 1, 0);
            this.tlpMiddle.Controls.Add(this.pnlLog, 2, 0);
            this.tlpMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMiddle.Location = new System.Drawing.Point(10, 80);
            this.tlpMiddle.Margin = new System.Windows.Forms.Padding(10);
            this.tlpMiddle.Name = "tlpMiddle";
            this.tlpMiddle.RowCount = 1;
            this.tlpMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMiddle.Size = new System.Drawing.Size(1380, 354);
            this.tlpMiddle.TabIndex = 1;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            this.pnlDetail.Controls.Add(this.lblDetailHeader);
            this.pnlDetail.Controls.Add(this.tlpDetail);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Padding = new System.Windows.Forms.Padding(16);
            this.pnlDetail.Size = new System.Drawing.Size(473, 354);
            this.pnlDetail.TabIndex = 0;
            // 
            // lblDetailHeader
            // 
            this.lblDetailHeader.AutoSize = true;
            this.lblDetailHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDetailHeader.ForeColor = System.Drawing.Color.White;
            this.lblDetailHeader.Location = new System.Drawing.Point(16, 16);
            this.lblDetailHeader.Name = "lblDetailHeader";
            this.lblDetailHeader.Size = new System.Drawing.Size(78, 25);
            this.lblDetailHeader.TabIndex = 0;
            this.lblDetailHeader.Text = "세부정보";
            // 
            // tlpDetail
            // 
            this.tlpDetail.ColumnCount = 2;
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tlpDetail.Controls.Add(this.lblProductTitle, 0, 0);
            this.tlpDetail.Controls.Add(this.lblEquipmentTitle, 0, 1);
            this.tlpDetail.Controls.Add(this.lblProgressTitle, 0, 2);
            this.tlpDetail.Controls.Add(this.lblSealerWidthTitle, 0, 3);
            this.tlpDetail.Controls.Add(this.lblScoreTitle, 0, 4);
            this.tlpDetail.Controls.Add(this.lblShortCircuitTitle, 0, 5);
            this.tlpDetail.Controls.Add(this.lblProductValue, 1, 0);
            this.tlpDetail.Controls.Add(this.lblEquipmentValue, 1, 1);
            this.tlpDetail.Controls.Add(this.lblProgressValue, 1, 2);
            this.tlpDetail.Controls.Add(this.lblSealerWidthValue, 1, 3);
            this.tlpDetail.Controls.Add(this.lblScoreValue, 1, 4);
            this.tlpDetail.Controls.Add(this.lblShortCircuitValue, 1, 5);
            this.tlpDetail.Location = new System.Drawing.Point(16, 56);
            this.tlpDetail.Name = "tlpDetail";
            this.tlpDetail.RowCount = 6;
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.7F));
            this.tlpDetail.Size = new System.Drawing.Size(441, 270);
            this.tlpDetail.TabIndex = 1;
            // 
            // lblProductTitle
            // 
            this.lblProductTitle.AutoSize = true;
            this.lblProductTitle.ForeColor = System.Drawing.Color.White;
            this.lblProductTitle.Location = new System.Drawing.Point(3, 0);
            this.lblProductTitle.Name = "lblProductTitle";
            this.lblProductTitle.Size = new System.Drawing.Size(44, 20);
            this.lblProductTitle.TabIndex = 0;
            this.lblProductTitle.Text = "제품";
            // 
            // lblEquipmentTitle
            // 
            this.lblEquipmentTitle.AutoSize = true;
            this.lblEquipmentTitle.ForeColor = System.Drawing.Color.White;
            this.lblEquipmentTitle.Location = new System.Drawing.Point(3, 45);
            this.lblEquipmentTitle.Name = "lblEquipmentTitle";
            this.lblEquipmentTitle.Size = new System.Drawing.Size(44, 20);
            this.lblEquipmentTitle.TabIndex = 1;
            this.lblEquipmentTitle.Text = "설비";
            // 
            // lblProgressTitle
            // 
            this.lblProgressTitle.AutoSize = true;
            this.lblProgressTitle.ForeColor = System.Drawing.Color.White;
            this.lblProgressTitle.Location = new System.Drawing.Point(3, 90);
            this.lblProgressTitle.Name = "lblProgressTitle";
            this.lblProgressTitle.Size = new System.Drawing.Size(57, 20);
            this.lblProgressTitle.TabIndex = 2;
            this.lblProgressTitle.Text = "진행률";
            // 
            // lblSealerWidthTitle
            // 
            this.lblSealerWidthTitle.AutoSize = true;
            this.lblSealerWidthTitle.ForeColor = System.Drawing.Color.White;
            this.lblSealerWidthTitle.Location = new System.Drawing.Point(3, 135);
            this.lblSealerWidthTitle.Name = "lblSealerWidthTitle";
            this.lblSealerWidthTitle.Size = new System.Drawing.Size(57, 20);
            this.lblSealerWidthTitle.TabIndex = 3;
            this.lblSealerWidthTitle.Text = "실러폭";
            // 
            // lblScoreTitle
            // 
            this.lblScoreTitle.AutoSize = true;
            this.lblScoreTitle.ForeColor = System.Drawing.Color.White;
            this.lblScoreTitle.Location = new System.Drawing.Point(3, 180);
            this.lblScoreTitle.Name = "lblScoreTitle";
            this.lblScoreTitle.Size = new System.Drawing.Size(44, 20);
            this.lblScoreTitle.TabIndex = 4;
            this.lblScoreTitle.Text = "Score";
            // 
            // lblShortCircuitTitle
            // 
            this.lblShortCircuitTitle.AutoSize = true;
            this.lblShortCircuitTitle.ForeColor = System.Drawing.Color.White;
            this.lblShortCircuitTitle.Location = new System.Drawing.Point(3, 225);
            this.lblShortCircuitTitle.Name = "lblShortCircuitTitle";
            this.lblShortCircuitTitle.Size = new System.Drawing.Size(44, 20);
            this.lblShortCircuitTitle.TabIndex = 5;
            this.lblShortCircuitTitle.Text = "단락";
            // 
            // lblProductValue
            // 
            this.lblProductValue.AutoSize = true;
            this.lblProductValue.ForeColor = System.Drawing.Color.Lime;
            this.lblProductValue.Location = new System.Drawing.Point(157, 0);
            this.lblProductValue.Name = "lblProductValue";
            this.lblProductValue.Size = new System.Drawing.Size(75, 20);
            this.lblProductValue.TabIndex = 6;
            this.lblProductValue.Text = "-";
            // 
            // lblEquipmentValue
            // 
            this.lblEquipmentValue.AutoSize = true;
            this.lblEquipmentValue.ForeColor = System.Drawing.Color.Lime;
            this.lblEquipmentValue.Location = new System.Drawing.Point(157, 45);
            this.lblEquipmentValue.Name = "lblEquipmentValue";
            this.lblEquipmentValue.Size = new System.Drawing.Size(75, 20);
            this.lblEquipmentValue.TabIndex = 7;
            this.lblEquipmentValue.Text = "-";
            // 
            // lblProgressValue
            // 
            this.lblProgressValue.AutoSize = true;
            this.lblProgressValue.ForeColor = System.Drawing.Color.Lime;
            this.lblProgressValue.Location = new System.Drawing.Point(157, 90);
            this.lblProgressValue.Name = "lblProgressValue";
            this.lblProgressValue.Size = new System.Drawing.Size(75, 20);
            this.lblProgressValue.TabIndex = 8;
            this.lblProgressValue.Text = "-";
            // 
            // lblSealerWidthValue
            // 
            this.lblSealerWidthValue.AutoSize = true;
            this.lblSealerWidthValue.ForeColor = System.Drawing.Color.Lime;
            this.lblSealerWidthValue.Location = new System.Drawing.Point(157, 135);
            this.lblSealerWidthValue.Name = "lblSealerWidthValue";
            this.lblSealerWidthValue.Size = new System.Drawing.Size(75, 20);
            this.lblSealerWidthValue.TabIndex = 9;
            this.lblSealerWidthValue.Text = "-";
            // 
            // lblScoreValue
            // 
            this.lblScoreValue.AutoSize = true;
            this.lblScoreValue.ForeColor = System.Drawing.Color.Lime;
            this.lblScoreValue.Location = new System.Drawing.Point(157, 180);
            this.lblScoreValue.Name = "lblScoreValue";
            this.lblScoreValue.Size = new System.Drawing.Size(75, 20);
            this.lblScoreValue.TabIndex = 10;
            this.lblScoreValue.Text = "-";
            // 
            // lblShortCircuitValue
            // 
            this.lblShortCircuitValue.AutoSize = true;
            this.lblShortCircuitValue.ForeColor = System.Drawing.Color.Lime;
            this.lblShortCircuitValue.Location = new System.Drawing.Point(157, 225);
            this.lblShortCircuitValue.Name = "lblShortCircuitValue";
            this.lblShortCircuitValue.Size = new System.Drawing.Size(75, 20);
            this.lblShortCircuitValue.TabIndex = 11;
            this.lblShortCircuitValue.Text = "-";
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            this.pnlChart.Controls.Add(this.lblChartHeader);
            this.pnlChart.Controls.Add(this.donutChart);
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(483, 0);
            this.pnlChart.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Padding = new System.Windows.Forms.Padding(16);
            this.pnlChart.Size = new System.Drawing.Size(335, 354);
            this.pnlChart.TabIndex = 1;
            // 
            // lblChartHeader
            // 
            this.lblChartHeader.AutoSize = true;
            this.lblChartHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblChartHeader.ForeColor = System.Drawing.Color.White;
            this.lblChartHeader.Location = new System.Drawing.Point(16, 16);
            this.lblChartHeader.Name = "lblChartHeader";
            this.lblChartHeader.Size = new System.Drawing.Size(78, 25);
            this.lblChartHeader.TabIndex = 0;
            this.lblChartHeader.Text = "생산현황";
            // 
            // donutChart
            // 
            this.donutChart.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            this.donutChart.Location = new System.Drawing.Point(30, 60);
            this.donutChart.Name = "donutChart";
            this.donutChart.ProductionNg = 0;
            this.donutChart.ProductionOk = 0;
            this.donutChart.ProductionTotal = 0;
            this.donutChart.Size = new System.Drawing.Size(260, 260);
            this.donutChart.TabIndex = 1;
            // 
            // pnlLog
            // 
            this.pnlLog.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            this.pnlLog.Controls.Add(this.rtbSystemLog);
            this.pnlLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLog.Location = new System.Drawing.Point(828, 0);
            this.pnlLog.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Padding = new System.Windows.Forms.Padding(16);
            this.pnlLog.Size = new System.Drawing.Size(552, 354);
            this.pnlLog.TabIndex = 2;
            // 
            // rtbSystemLog
            // 
            this.rtbSystemLog.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.rtbSystemLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbSystemLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSystemLog.ForeColor = System.Drawing.Color.White;
            this.rtbSystemLog.Location = new System.Drawing.Point(16, 16);
            this.rtbSystemLog.Name = "rtbSystemLog";
            this.rtbSystemLog.Size = new System.Drawing.Size(520, 322);
            this.rtbSystemLog.TabIndex = 0;
            this.rtbSystemLog.Text = "[SYSTEM] Application Started Successfully.\n[PLC] Communication Link Established.\n[INSPECT] Ready for Sealer Inspection.";
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 2;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.Controls.Add(this.pnlCam1, 0, 0);
            this.tlpBottom.Controls.Add(this.pnlCam2, 1, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(10, 454);
            this.tlpBottom.Margin = new System.Windows.Forms.Padding(10);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(1380, 436);
            this.tlpBottom.TabIndex = 2;
            // 
            // pnlCam1
            // 
            this.pnlCam1.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            this.pnlCam1.Controls.Add(this.lblCam1Header);
            this.pnlCam1.Controls.Add(this.pbCam1);
            this.pnlCam1.Controls.Add(this.lblCam1Result);
            this.pnlCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCam1.Location = new System.Drawing.Point(0, 0);
            this.pnlCam1.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlCam1.Name = "pnlCam1";
            this.pnlCam1.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCam1.Size = new System.Drawing.Size(680, 436);
            this.pnlCam1.TabIndex = 0;
            // 
            // lblCam1Header
            // 
            this.lblCam1Header.AutoSize = true;
            this.lblCam1Header.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCam1Header.ForeColor = System.Drawing.Color.White;
            this.lblCam1Header.Location = new System.Drawing.Point(16, 16);
            this.lblCam1Header.Name = "lblCam1Header";
            this.lblCam1Header.Size = new System.Drawing.Size(89, 25);
            this.lblCam1Header.TabIndex = 0;
            this.lblCam1Header.Text = "Camera 1";
            // 
            // pbCam1
            // 
            this.pbCam1.BackColor = System.Drawing.Color.Black;
            this.pbCam1.Location = new System.Drawing.Point(16, 52);
            this.pbCam1.Name = "pbCam1";
            this.pbCam1.Size = new System.Drawing.Size(648, 320);
            this.pbCam1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCam1.TabIndex = 1;
            this.pbCam1.TabStop = false;
            // 
            // lblCam1Result
            // 
            this.lblCam1Result.AutoSize = true;
            this.lblCam1Result.ForeColor = System.Drawing.Color.Lime;
            this.lblCam1Result.Location = new System.Drawing.Point(16, 386);
            this.lblCam1Result.Name = "lblCam1Result";
            this.lblCam1Result.Size = new System.Drawing.Size(41, 20);
            this.lblCam1Result.TabIndex = 2;
            this.lblCam1Result.Text = "CAM01";
            // 
            // pnlCam2
            // 
            this.pnlCam2.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
            this.pnlCam2.Controls.Add(this.lblCam2Header);
            this.pnlCam2.Controls.Add(this.pbCam2);
            this.pnlCam2.Controls.Add(this.lblCam2Result);
            this.pnlCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCam2.Location = new System.Drawing.Point(690, 0);
            this.pnlCam2.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCam2.Name = "pnlCam2";
            this.pnlCam2.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCam2.Size = new System.Drawing.Size(690, 436);
            this.pnlCam2.TabIndex = 1;
            // 
            // lblCam2Header
            // 
            this.lblCam2Header.AutoSize = true;
            this.lblCam2Header.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCam2Header.ForeColor = System.Drawing.Color.White;
            this.lblCam2Header.Location = new System.Drawing.Point(16, 16);
            this.lblCam2Header.Name = "lblCam2Header";
            this.lblCam2Header.Size = new System.Drawing.Size(89, 25);
            this.lblCam2Header.TabIndex = 0;
            this.lblCam2Header.Text = "Camera 2";
            // 
            // pbCam2
            // 
            this.pbCam2.BackColor = System.Drawing.Color.Black;
            this.pbCam2.Location = new System.Drawing.Point(16, 52);
            this.pbCam2.Name = "pbCam2";
            this.pbCam2.Size = new System.Drawing.Size(658, 320);
            this.pbCam2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCam2.TabIndex = 1;
            this.pbCam2.TabStop = false;
            // 
            // lblCam2Result
            // 
            this.lblCam2Result.AutoSize = true;
            this.lblCam2Result.ForeColor = System.Drawing.Color.Red;
            this.lblCam2Result.Location = new System.Drawing.Point(16, 386);
            this.lblCam2Result.Name = "lblCam2Result";
            this.lblCam2Result.Size = new System.Drawing.Size(41, 20);
            this.lblCam2Result.TabIndex = 2;
            this.lblCam2Result.Text = "CAM02";
            // 
            // SealerBeadMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.tlpRoot);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SealerBeadMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sealer Bead Inspection System";
            this.Shown += new System.EventHandler(this.SealerBeadMainForm_Shown);
            this.tlpRoot.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlTopButtons.ResumeLayout(false);
            this.flpOutputs.ResumeLayout(false);
            this.flpOutputs.PerformLayout();
            this.flpInputs.ResumeLayout(false);
            this.flpInputs.PerformLayout();
            this.tlpMiddle.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            this.tlpDetail.ResumeLayout(false);
            this.tlpDetail.PerformLayout();
            this.pnlChart.ResumeLayout(false);
            this.pnlChart.PerformLayout();
            this.pnlLog.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            this.pnlCam1.ResumeLayout(false);
            this.pnlCam1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCam1)).EndInit();
            this.pnlCam2.ResumeLayout(false);
            this.pnlCam2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCam2)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
