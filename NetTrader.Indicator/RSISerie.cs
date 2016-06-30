using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class RSISerie : IIndicatorSerie
    {
        public List<double?> RSI { get; set; }
        public List<double?> RS { get; set; }

        public RSISerie()
        {
            RSI = new List<double?>();
            RS = new List<double?>();
        }
    }
}
