using AvisSealer.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisSealer.datas
{
    public class MainGrid
    {
        public int index;

        public string Index
        {
            get
            {
                return index.ToString();
            }
        }

        public string InspTitle
        {
            get;
            set;
        }

        public double inspDuration_ms;

        public string InspDuration
        {
            get
            {
                if (inspDuration_ms > 0) return string.Format("{0}ms", inspDuration_ms);

                return "-";
            }
        }

        public CommonRepository.EnumInspResult result;

        public Image Result
        {
            get
            {
                if (result == CommonRepository.EnumInspResult.Ok) return Properties.Resources.icon_pass;
                else if (result == CommonRepository.EnumInspResult.Ng) return Properties.Resources.icon_fail;

                return Properties.Resources.icon_check;
            }
        }

        public ResultItem resultItem = null;

        public MainGrid()
        {
            Clear();
        }

        public MainGrid(int index, string inspTitle, double inspDuration_ms, CommonRepository.EnumInspResult result)
        {
            Set(index, inspTitle, inspDuration_ms, result);
        }

        public void Set(int index, string inspTitle, double inspDuration_ms, CommonRepository.EnumInspResult result)
        {
            this.index = index;
            this.InspTitle = inspTitle;
            this.inspDuration_ms = inspDuration_ms;
            this.result = result;
        }

        public void Clear()
        {
            Set(0, "Unknown", 0, CommonRepository.EnumInspResult.Unknown);
        }
    }
}
