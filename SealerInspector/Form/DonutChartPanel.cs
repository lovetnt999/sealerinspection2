using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AvisSealer
{
    public class DonutChartPanel : Panel
    {
        private int _productionOk;
        private int _productionNg;
        private int _productionTotal;

        public int ProductionOk
        {
            get => _productionOk;
            set
            {
                _productionOk = value;
                Invalidate();
            }
        }

        public int ProductionNg
        {
            get => _productionNg;
            set
            {
                _productionNg = value;
                Invalidate();
            }
        }

        public int ProductionTotal
        {
            get => _productionTotal;
            set
            {
                _productionTotal = value;
                Invalidate();
            }
        }

        public DonutChartPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int size = Math.Min(Width, Height) - 10;
            Rectangle rect = new Rectangle((Width - size) / 2, (Height - size) / 2, size, size);

            int total = Math.Max(1, ProductionTotal);
            float okSweep = (float)ProductionOk / total * 360f;
            float ngSweep = (float)ProductionNg / total * 360f;

            using (Pen basePen = new Pen(Color.FromArgb(60, 60, 60), 20))
            using (Pen okPen = new Pen(Color.Lime, 20))
            using (Pen ngPen = new Pen(Color.Red, 20))
            {
                e.Graphics.DrawArc(basePen, rect, 0, 360);
                e.Graphics.DrawArc(okPen, rect, -90, okSweep);
                e.Graphics.DrawArc(ngPen, rect, -90 + okSweep, ngSweep);
            }

            string centerText = ProductionTotal.ToString("D5");
            using (Font font = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.White))
            {
                SizeF textSize = e.Graphics.MeasureString(centerText, font);
                e.Graphics.DrawString(centerText, font, brush, (Width - textSize.Width) / 2, (Height - textSize.Height) / 2);
            }

            using (Font font = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (Brush okBrush = new SolidBrush(Color.Lime))
            using (Brush ngBrush = new SolidBrush(Color.Red))
            {
                e.Graphics.DrawString($"OK {ProductionOk:D4}", font, okBrush, 10, 10);
                e.Graphics.DrawString($"NG {ProductionNg:D4}", font, ngBrush, 10, 28);
            }
        }
    }
}
