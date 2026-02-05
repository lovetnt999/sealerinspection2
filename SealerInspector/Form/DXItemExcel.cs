using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using AvisSealer.Server;

namespace AvisSealer
{
    public partial class DXItemExcel : DevExpress.XtraEditors.XtraForm
    {
        private SealerResults _results = null;
        
        public DXItemExcel(SealerResults results)
        {
            InitializeComponent();

            this._results = results;
        }

        private void DXItemExcel_Load(object sender, EventArgs e)
        {
            try
            {
                this.Size = new Size(1000, 600);

                // init
                for (var i = 1; i < spreadsheetControl.Document.Worksheets.Count; i++)
                {
                    spreadsheetControl.Document.Worksheets.RemoveAt(i);
                }
                spreadsheetControl.Document.Worksheets.Add();
                spreadsheetControl.Document.Worksheets.RemoveAt(0);
                spreadsheetControl.Document.Worksheets.ActiveWorksheet = spreadsheetControl.Document.Worksheets[0];



                DevExpress.Spreadsheet.Worksheet ws = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
                ws.Name = DateTime.Now.ToString("yyyyMMdd_HHmmss");


                ProgramConfig config = ProgramConfig.Instance();
                CommonRepository repository = CommonRepository.Instance();


                InputString(ws.Range["A1"], "순번", Color.Black, Color.LightGray, 150);
                InputString(ws.Range["B1"], "시작시간", Color.Black, Color.LightGray, 450);
                InputString(ws.Range["C1"], "종료시간", Color.Black, Color.LightGray, 450);
                InputString(ws.Range["D1"], "검사시간", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["E1"], "모델", Color.Black, Color.LightGray, 330);
                InputString(ws.Range["F1"], "PCB코드", Color.Black, Color.LightGray, 380);
                InputString(ws.Range["G1"], "스펙라벨", Color.Black, Color.LightGray, 430);
                InputString(ws.Range["H1"], "F/W", Color.Black, Color.LightGray, 380);
                InputString(ws.Range["I1"], "결과", Color.Black, Color.LightGray, 350);
                InputString(ws.Range["J1"], "기본검사", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["K1"], "LED(PWR)", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["L1"], "LED(SYS)", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["M1"], "LED(CAM)", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["N1"], "LED(SW)", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["O1"], "출력전원1", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["P1"], "출력전원2", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["Q1"], "전면 스위치", Color.Black, Color.LightGray, 300);
                InputString(ws.Range["R1"], "영상인식(수배차량)", Color.Black, Color.LightGray, 350);
                InputString(ws.Range["S1"], "영상인식(미납차량)", Color.Black, Color.LightGray, 350);
                InputString(ws.Range["T1"], "영상인식(일반차량)", Color.Black, Color.LightGray, 350);


                if (_results != null && _results.listResult != null)
                {
                    int idx = 2;
                    foreach (SealerResult r in _results.listResult)
                    {
                        SealerResult item = ServerManager.GetResultDetail(r.ResultSeq);

                        if (item != null)
                        {
                            CommonRepository.EnumInspResult s1 = item.listItem[7].itemResult;
                            CommonRepository.EnumInspResult s2 = item.listItem[8].itemResult;
                            CommonRepository.EnumInspResult s3 = item.listItem[9].itemResult;
                            CommonRepository.EnumInspResult s4 = item.listItem[10].itemResult;
                            CommonRepository.EnumInspResult s5 = item.listItem[11].itemResult;
                            CommonRepository.EnumInspResult s6 = item.listItem[12].itemResult;
                            CommonRepository.EnumInspResult s7 = item.listItem[13].itemResult;
                            bool sResult = (s1 == CommonRepository.EnumInspResult.Ok) && (s2 == CommonRepository.EnumInspResult.Ok) && (s3 == CommonRepository.EnumInspResult.Ok) && (s4 == CommonRepository.EnumInspResult.Ok) && (s5 == CommonRepository.EnumInspResult.Ok) && (s6 == CommonRepository.EnumInspResult.Ok) && (s7 == CommonRepository.EnumInspResult.Ok);


                            InputString(ws.Range["A" + idx], (idx - 1).ToString(), Color.Black, Color.White);
                            InputString(ws.Range["B" + idx], item.StartTime, Color.Black, Color.White);
                            InputString(ws.Range["C" + idx], item.InspEndTime, Color.Black, Color.White);
                            InputString(ws.Range["D" + idx], item.InspDurationTimeMilliSec + "ms", Color.Black, Color.White);
                            InputString(ws.Range["E" + idx], repository.GetModelName(r.Model), Color.Black, Color.White);
                            InputString(ws.Range["F" + idx], item.Barcode, Color.Black, Color.White);
                            InputString(ws.Range["G" + idx], item.OutBarcode, Color.Black, Color.White);
                            InputString(ws.Range["H" + idx], item.firmwareVersion, Color.Black, Color.White);

                            InputString(ws.Range["I" + idx], item.totalResult == CommonRepository.EnumInspResult.Ok);

                            InputString(ws.Range["J" + idx], item.listItem[0].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["K" + idx], item.listItem[1].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["L" + idx], item.listItem[2].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["M" + idx], item.listItem[3].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["N" + idx], item.listItem[4].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["O" + idx], item.listItem[5].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["P" + idx], item.listItem[6].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["Q" + idx], sResult);
                            InputString(ws.Range["R" + idx], item.listItem[14].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["S" + idx], item.listItem[15].itemResult == CommonRepository.EnumInspResult.Ok);
                            InputString(ws.Range["T" + idx], item.listItem[16].itemResult == CommonRepository.EnumInspResult.Ok);
                        }

                        idx++;
                    }
                }
            }
            catch (Exception) { }
        }

        private void InputString(DevExpress.Spreadsheet.Range range, bool isOk)
        {
            InputString(range, isOk ? "OK" : "NG",
                DevExpress.Spreadsheet.BorderLineStyle.Thin,
                DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center,
                DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment.Center,
                isOk ? Color.Black : Color.White,
                isOk ? Color.White : Color.Red);
        }

        private void InputString(DevExpress.Spreadsheet.Range range, string value, bool isOk)
        {
            InputString(range, value,
                DevExpress.Spreadsheet.BorderLineStyle.Thin,
                DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center,
                DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment.Center,
                isOk ? Color.Black : Color.White,
                isOk ? Color.White : Color.Red);
        }

        private void InputString(DevExpress.Spreadsheet.Range range, string value, Color fontColor, Color bgColor)
        {
            InputString(range, value,
                DevExpress.Spreadsheet.BorderLineStyle.Thin,
                DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center,
                DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment.Center,
                fontColor,
                bgColor);
        }

        private void InputString(DevExpress.Spreadsheet.Range range, string value, Color fontColor, Color bgColor, int width)
        {
            InputString(range, value,
                DevExpress.Spreadsheet.BorderLineStyle.Thin,
                DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Center,
                DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment.Center,
                fontColor,
                bgColor);

            range.ColumnWidth = width;
        }

        private void InputString(
            DevExpress.Spreadsheet.Range range,
            string value,
            DevExpress.Spreadsheet.BorderLineStyle lineStyle,
            DevExpress.Spreadsheet.SpreadsheetVerticalAlignment alignmentV,
            DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment alignmentH,
            Color fontColor,
            Color bgColor)
        {
            range.Value = value;
            range.Alignment.Horizontal = alignmentH;
            range.Alignment.Vertical = alignmentV;

            if (fontColor != Color.Transparent) range.Font.Color = fontColor;
            if (bgColor != Color.Transparent) range.FillColor = bgColor;

            if (lineStyle != DevExpress.Spreadsheet.BorderLineStyle.None) SetOutsideline(range, lineStyle);
        }
        
        private void SetOutsideline(DevExpress.Spreadsheet.Range range, DevExpress.Spreadsheet.BorderLineStyle outLineStyle)
        {
            range.Borders.TopBorder.LineStyle = outLineStyle;
            range.Borders.BottomBorder.LineStyle = outLineStyle;
            range.Borders.LeftBorder.LineStyle = outLineStyle;
            range.Borders.RightBorder.LineStyle = outLineStyle;
        }

        private void SetInsideline(DevExpress.Spreadsheet.Range range, DevExpress.Spreadsheet.BorderLineStyle verLineStyle, DevExpress.Spreadsheet.BorderLineStyle horLineStyle)
        {
            range.Borders.InsideVerticalBorders.LineStyle = verLineStyle;
            range.Borders.InsideHorizontalBorders.LineStyle = horLineStyle;
        }

        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                spreadsheetControl.SaveDocumentAs();
            }
            catch (System.Exception)
            {
            }
        }
    }
}
