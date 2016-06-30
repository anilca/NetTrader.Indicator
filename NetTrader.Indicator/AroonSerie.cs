using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class AroonSerie : IIndicatorSerie
    {
        public List<double?> Up { get; private set; }
        public List<double?> Down { get; private set; }

        public AroonSerie()
        {
            Up = new List<double?>();
            Down = new List<double?>();
        }
    }
}
