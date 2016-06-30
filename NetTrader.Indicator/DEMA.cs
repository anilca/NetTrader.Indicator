using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Double Exponential Moving Average (DEMA)
    /// </summary>
    public class DEMA : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        public int Period { get; set; }

        public DEMA(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// DEMA = 2 * EMA - EMA of EMA
        /// </summary>
        /// <see cref="http://forex-indicators.net/trend-indicators/dema"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie demaSerie = new SingleDoubleSerie();
            EMA ema = new EMA(Period, false);
            ema.Load(OhlcList);
            List<double?> emaValues = (ema.Calculate() as SingleDoubleSerie).Values;

            // assign EMA values to Close price
            for (int i = 0; i < OhlcList.Count; i++)
            {
                OhlcList[i].Close = emaValues[i].HasValue ? emaValues[i].Value : 0.0;
            }

            ema.Load(OhlcList.Skip(Period - 1).ToList());
            // EMA(EMA(value))
            List<double?> emaEmaValues = (ema.Calculate() as SingleDoubleSerie).Values;
            for (int i = 0; i < Period - 1; i++)
            {
                emaEmaValues.Insert(0, null);
            }    

            // Calculate DEMA
            for (int i = 0; i < OhlcList.Count; i++) 
            {
                if (i >= 2 * Period - 2)
                {
                    var dema = 2 * emaValues[i] - emaEmaValues[i];
                    demaSerie.Values.Add(dema);
                }
                else
                {
                    demaSerie.Values.Add(null);
                }
            }

            return demaSerie;
        }
    }
}
