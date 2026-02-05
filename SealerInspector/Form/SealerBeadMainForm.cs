using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AvisSealer
{
    public partial class SealerBeadMainForm : Form
    {
        private readonly bool[] _inputs = new bool[6];
        private readonly bool[] _outputs = new bool[3];
        private readonly List<Label> _inputIndicators = new List<Label>();
        private readonly List<Label> _outputIndicators = new List<Label>();
        private readonly Dictionary<string, Label> _detailValueLabels = new Dictionary<string, Label>();

        public SealerBeadMainForm()
        {
            InitializeComponent();
            InitializeIndicatorLists();
            InitializeDetailLabels();
            UpdatePLCStatus();
        }

        private void InitializeIndicatorLists()
        {
            _inputIndicators.AddRange(new[] { lblInput01, lblInput02, lblInput03, lblInput04, lblInput05, lblInput06 });
            _outputIndicators.AddRange(new[] { lblOutput01, lblOutput02, lblOutput03 });
        }

        private void InitializeDetailLabels()
        {
            _detailValueLabels["Product"] = lblProductValue;
            _detailValueLabels["Equipment"] = lblEquipmentValue;
            _detailValueLabels["Progress"] = lblProgressValue;
            _detailValueLabels["SealerWidth"] = lblSealerWidthValue;
            _detailValueLabels["Score"] = lblScoreValue;
            _detailValueLabels["ShortCircuit"] = lblShortCircuitValue;
        }

        public void UpdatePLCStatus()
        {
            UpdateIndicatorColors(_inputIndicators, _inputs);
            UpdateIndicatorColors(_outputIndicators, _outputs);
        }

        private void UpdateIndicatorColors(List<Label> indicators, bool[] states)
        {
            for (int i = 0; i < indicators.Count; i++)
            {
                bool isOn = i < states.Length && states[i];
                indicators[i].BackColor = isOn ? Color.Lime : Color.FromArgb(50, 50, 50);
                indicators[i].ForeColor = isOn ? Color.Black : Color.White;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("프로그램을 종료하시겠습니까?", "종료 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            using (SettingForm form = new SettingForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            using (HistoryForm form = new HistoryForm())
            {
                form.ShowDialog(this);
            }
        }

        private void SealerBeadMainForm_Shown(object sender, EventArgs e)
        {
            StartInspection();
        }

        public void StartInspection()
        {
            InspectionDetailData detailData = new InspectionDetailData
            {
                ProductName = "Side OTR LHD",
                Equipment = "R12",
                Progress = "34.5% (353EA)",
                SealerWidth = "5.43mm (기준: 3~8mm)",
                Score = "96.5% (기준: 93.5% ↑)",
                ShortCircuit = "None (기준: None)"
            };

            UpdateDetailLabels(detailData);
            UpdateCameraImage(pbCam1, @"C:\Images\cam1.jpg");
            UpdateCameraImage(pbCam2, @"C:\Images\cam2.jpg");

            lblCam1Result.Text = "CAM01 : 실러폭 5.43mm / Score 96.5%";
            lblCam2Result.Text = "CAM02 : 실러폭 0.00mm / Score 0.0%";

            _inputs[0] = true;
            _inputs[1] = true;
            _outputs[0] = true;
            UpdatePLCStatus();

            donutChart.ProductionOk = 1495;
            donutChart.ProductionNg = 5;
            donutChart.ProductionTotal = 1500;
        }

        private void UpdateCameraImage(PictureBox pictureBox, string imagePath)
        {
            if (File.Exists(imagePath))
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    pictureBox.Image = new Bitmap(image);
                }
            }
            else
            {
                pictureBox.Image = null;
            }
        }

        private void UpdateDetailLabels(InspectionDetailData data)
        {
            _detailValueLabels["Product"].Text = data.ProductName;
            _detailValueLabels["Equipment"].Text = data.Equipment;
            _detailValueLabels["Progress"].Text = data.Progress;
            _detailValueLabels["SealerWidth"].Text = data.SealerWidth;
            _detailValueLabels["Score"].Text = data.Score;
            _detailValueLabels["ShortCircuit"].Text = data.ShortCircuit;
        }

        public class InspectionDetailData
        {
            public string ProductName { get; set; }
            public string Equipment { get; set; }
            public string Progress { get; set; }
            public string SealerWidth { get; set; }
            public string Score { get; set; }
            public string ShortCircuit { get; set; }
        }
    }
}
