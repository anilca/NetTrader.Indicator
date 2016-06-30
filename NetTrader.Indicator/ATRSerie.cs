using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class ATRSerie : IIndicatorSerie
    {
        public List<double?> TrueHigh { get; private set; }
        public List<double?> TrueLow { get; private set; }
        public List<double?> TrueRange { get; private set; }
        public List<double?> ATR { get; private set; }

        public ATRSerie()
        {
            TrueHigh = new List<double?>();
            TrueLow = new List<double?>();
            TrueRange = new List<double?>();
            ATR = new List<double?>();
        }
    }
}
