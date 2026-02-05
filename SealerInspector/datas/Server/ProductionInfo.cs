using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvisSealer.Server
{
    [XmlRoot("ProductionInfo")]
    public class ProductionInfo
    {
        [XmlAttribute("production_info_seq")]
        public int ProdSeq { get; set; }

        [XmlAttribute("production_goal_count")]
        public int GoalCnt { get; set; }

        [XmlAttribute("production_current_count")]
        public int CurrCnt
        {
            get
            {
                return OkCnt + NgCnt;
            }
        }

        [XmlAttribute("production_ok_count")]
        public int OkCnt { get; set; }

        [XmlAttribute("production_ng_count")]
        public int NgCnt { get; set; }

        [XmlAttribute("production_serial_code")]
        public int SerialCode { get; set; }


        public ProductionInfo()
        {
            Clear();
        }

        public ProductionInfo(int ProdSeq, int GoalCnt, int OkCnt, int NgCnt, int SerialCode)
        {
            Set(ProdSeq, GoalCnt, OkCnt, NgCnt, SerialCode);
        }

        public void Clear()
        {
            Set(-1, 0, 0, 0, 0);
        }

        public void Set(int ProdSeq, int GoalCnt, int OkCnt, int NgCnt, int SerialCode)
        {
            this.ProdSeq = ProdSeq;
            this.GoalCnt = GoalCnt;
            this.OkCnt = OkCnt;
            this.NgCnt = NgCnt;
            this.SerialCode = SerialCode;
        }

        public void Set(ProductionInfo src)
        {
            Clear();
            if (src != null)
            {
                Set(src.ProdSeq, src.GoalCnt, src.OkCnt, src.NgCnt, src.SerialCode);
            }
        }
    }
}
