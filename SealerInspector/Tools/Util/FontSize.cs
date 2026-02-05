using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wamc.Tools.Util
{
    public class FontSize
    {
        public static string fontFace = "나눔고딕";

        public static System.Drawing.Font GetDefaultFont(bool isBold)
        {
            return new System.Drawing.Font(FontSize.fontFace, 10, isBold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular);
        }

        public static System.Drawing.Font AutoFontSize(object label, bool isBold, int wPadding, int hPadding)
        {
            if (label == null)
            {
                return GetDefaultFont(isBold);
            }

            if (label.GetType() == typeof(DevExpress.XtraLayout.LayoutControlItem))
            {
                DevExpress.XtraLayout.LayoutControlItem labelItem = (DevExpress.XtraLayout.LayoutControlItem)label;

                using (System.Drawing.Graphics gp = labelItem.Control.CreateGraphics())
                {
                    return AutoFontSize(labelItem.AppearanceItemCaption.Font, gp, labelItem.TextSize.Width - (wPadding * 2), labelItem.TextSize.Height - (hPadding * 2), labelItem.Text, isBold);
                }
            }
            else if (label.GetType() == typeof(DevExpress.XtraEditors.LabelControl))
            {
                DevExpress.XtraEditors.LabelControl labelItem = (DevExpress.XtraEditors.LabelControl)label;

                using (System.Drawing.Graphics gp = labelItem.CreateGraphics())
                {
                    return AutoFontSize(labelItem.Font, gp, labelItem.Width - (wPadding * 2), labelItem.Height - (hPadding * 2), labelItem.Text, isBold);
                }
            }

            return GetDefaultFont(isBold);
        }

        public static System.Drawing.Size GetStringSize(IntPtr handle, System.Drawing.Font font, string str, int widthMargin, int heightMargin)
        {
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(handle);
            System.Drawing.SizeF s = g.MeasureString(str, font);
            return new System.Drawing.Size((int)s.Width + widthMargin, (int)s.Height + heightMargin);
        }

        public static System.Drawing.Font AutoFontSize(System.Drawing.Font src, System.Drawing.Graphics gp, int width, int height, String text, bool isBold)
        {
            if (gp == null)
            {
                return new System.Drawing.Font(FontSize.fontFace, 10, isBold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular);
            }

            if (string.IsNullOrEmpty(text))
            {
                text = "-";
            }

            System.Drawing.SizeF textSize = gp.MeasureString(text, src);

            Single Faktor;
            Single FaktorX = (width) / textSize.Width;
            Single FaktorY = (height) / textSize.Height;
            if (FaktorX > FaktorY)
                Faktor = FaktorY;
            else
                Faktor = FaktorX;

            return new System.Drawing.Font(FontSize.fontFace, src.SizeInPoints * (Faktor) - 1, isBold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular);
        }
    }
}
