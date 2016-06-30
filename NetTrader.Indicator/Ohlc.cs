using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class Ohlc
    {
        public DateTime Date { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public double AdjClose { get; set; }
    }
}
