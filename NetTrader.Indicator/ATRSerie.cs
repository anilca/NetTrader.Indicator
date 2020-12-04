using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class ATRSerie : IIndicatorSerie
    {
        public List<double?> TrueRange { get; private set; }
        public List<double?> ATR { get; private set; }

        public ATRSerie()
        {
            TrueRange = new List<double?>();
            ATR = new List<double?>();
        }
    }
}
