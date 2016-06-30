using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class Momentum : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }

        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie momentumSerie = new SingleDoubleSerie();
            momentumSerie.Values.Add(null);

            for (int i = 1; i < OhlcList.Count; i++)
            {
                momentumSerie.Values.Add(OhlcList[i].Close - OhlcList[i - 1].Close);    
            }

            return momentumSerie;
        }

        public SingleDoubleSerie Calculate(List<double> values)
        {
            SingleDoubleSerie momentumSerie = new SingleDoubleSerie();
            momentumSerie.Values.Add(null);

            for (int i = 1; i < values.Count; i++)
            {
                momentumSerie.Values.Add(values[i] - values[i - 1]);
            }

            return momentumSerie;
        }
    }
}
