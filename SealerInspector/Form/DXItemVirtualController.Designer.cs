namespace AvisSealer
{
    partial class DXItemVirtualController
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
            this.simpleButtonInspStart = new DevExpress.XtraEditors.SimpleButton();
            this.toggleSwitchCylinder = new DevExpress.XtraEditors.ToggleSwitch();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonClose = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textEditBarcode = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.toggleSwitchContinueInsp = new DevExpress.XtraEditors.ToggleSwitch();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.toggleSwitchLebelPrint = new DevExpress.XtraEditors.ToggleSwitch();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchCylinder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchContinueInsp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchLebelPrint.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButtonInspStart
            // 
            this.simpleButtonInspStart.AllowFocus = false;
            this.simpleButtonInspStart.Location = new System.Drawing.Point(21, 14);
            this.simpleButtonInspStart.Name = "simpleButtonInspStart";
            this.simpleButtonInspStart.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonInspStart.TabIndex = 0;
            this.simpleButtonInspStart.Text = "검사시작";
            this.simpleButtonInspStart.Click += new System.EventHandler(this.simpleButtonInspStart_Click);
            // 
            // toggleSwitchCylinder
            // 
            this.toggleSwitchCylinder.Location = new System.Drawing.Point(169, 14);
            this.toggleSwitchCylinder.Name = "toggleSwitchCylinder";
            this.toggleSwitchCylinder.Properties.AllowFocused = false;
            this.toggleSwitchCylinder.Properties.OffText = "Unpush";
            this.toggleSwitchCylinder.Properties.OnText = "Push";
            this.toggleSwitchCylinder.Size = new System.Drawing.Size(134, 25);
            this.toggleSwitchCylinder.TabIndex = 1;
            this.toggleSwitchCylinder.Toggled += new System.EventHandler(this.toggleSwitchCylinder_Toggled);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(121, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "실린더";
            // 
            // simpleButtonClose
            // 
            this.simpleButtonClose.AllowFocus = false;
            this.simpleButtonClose.Location = new System.Drawing.Point(319, 14);
            this.simpleButtonClose.Name = "simpleButtonClose";
            this.simpleButtonClose.Size = new System.Drawing.Size(52, 23);
            this.simpleButtonClose.TabIndex = 3;
            this.simpleButtonClose.Text = "닫기";
            this.simpleButtonClose.Click += new System.EventHandler(this.simpleButtonClose_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(121, 55);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(30, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "바코드";
            // 
            // textEditBarcode
            // 
            this.textEditBarcode.Location = new System.Drawing.Point(169, 51);
            this.textEditBarcode.Name = "textEditBarcode";
            this.textEditBarcode.Size = new System.Drawing.Size(134, 20);
            this.textEditBarcode.TabIndex = 5;
            this.textEditBarcode.EditValueChanged += new System.EventHandler(this.textEditBarcode_EditValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(121, 91);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "연속검사";
            // 
            // toggleSwitchContinueInsp
            // 
            this.toggleSwitchContinueInsp.Location = new System.Drawing.Point(169, 86);
            this.toggleSwitchContinueInsp.Name = "toggleSwitchContinueInsp";
            this.toggleSwitchContinueInsp.Properties.AllowFocused = false;
            this.toggleSwitchContinueInsp.Properties.OffText = "NO";
            this.toggleSwitchContinueInsp.Properties.OnText = "YES";
            this.toggleSwitchContinueInsp.Size = new System.Drawing.Size(134, 25);
            this.toggleSwitchContinueInsp.TabIndex = 6;
            this.toggleSwitchContinueInsp.Toggled += new System.EventHandler(this.toggleSwitchContinueInsp_Toggled);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(107, 129);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(54, 14);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "라벨 미출력";
            // 
            // toggleSwitchLebelPrint
            // 
            this.toggleSwitchLebelPrint.Location = new System.Drawing.Point(169, 124);
            this.toggleSwitchLebelPrint.Name = "toggleSwitchLebelPrint";
            this.toggleSwitchLebelPrint.Properties.AllowFocused = false;
            this.toggleSwitchLebelPrint.Properties.OffText = "NO";
            this.toggleSwitchLebelPrint.Properties.OnText = "YES";
            this.toggleSwitchLebelPrint.Size = new System.Drawing.Size(134, 25);
            this.toggleSwitchLebelPrint.TabIndex = 8;
            this.toggleSwitchLebelPrint.Toggled += new System.EventHandler(this.toggleSwitchLebelPrint_Toggled);
            // 
            // DXItemVirtualController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 161);
            this.ControlBox = false;
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.toggleSwitchLebelPrint);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.toggleSwitchContinueInsp);
            this.Controls.Add(this.textEditBarcode);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButtonClose);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.toggleSwitchCylinder);
            this.Controls.Add(this.simpleButtonInspStart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DXItemVirtualController";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "가상 리모컨";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DXItemVirtualController_Load);
            this.VisibleChanged += new System.EventHandler(this.DXItemVirtualController_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchCylinder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchContinueInsp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitchLebelPrint.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButtonInspStart;
        private DevExpress.XtraEditors.ToggleSwitch toggleSwitchCylinder;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonClose;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEditBarcode;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ToggleSwitch toggleSwitchContinueInsp;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ToggleSwitch toggleSwitchLebelPrint;
    }
}