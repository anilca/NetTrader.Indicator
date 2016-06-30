using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class IchimokuSerie: IIndicatorSerie
    {
        public List<double?> ConversionLine { get; set; }
        public List<double?> BaseLine { get; set; }
        public List<double?> LeadingSpanA { get; set; }
        public List<double?> LeadingSpanB { get; set; }
        public List<double?> LaggingSpan { get; set; }

        public IchimokuSerie()
        {
            ConversionLine = new List<double?>();
            BaseLine = new List<double?>();
            LeadingSpanA = new List<double?>();
            LeadingSpanB = new List<double?>();
            LaggingSpan = new List<double?>();
        }
    }
}
