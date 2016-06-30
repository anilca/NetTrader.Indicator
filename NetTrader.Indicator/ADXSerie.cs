using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class ADXSerie : IIndicatorSerie
    {
        public List<double?> TrueRange { get; set; }
        public List<double?> DINegative { get; set; }
        public List<double?> DIPositive { get; set; }
        public List<double?> DX { get; set; }
        public List<double?> ADX { get; set; }

        public ADXSerie()
        {
            TrueRange = new List<double?>();
            DINegative = new List<double?>();
            DIPositive = new List<double?>();
            DX = new List<double?>();
            ADX = new List<double?>();
        }
    }
}
