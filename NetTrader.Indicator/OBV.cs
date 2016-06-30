using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// On Balance Volume (OBV)
    /// </summary>
    public class OBV : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }

        /// <summary>
        /// If today’s close is greater than yesterday’s close then:
        /// OBV(i) = OBV(i-1)+VOLUME(i)
        /// If today’s close is less than yesterday’s close then:
        /// OBV(i) = OBV(i-1)-VOLUME(i)
        /// If today’s close is equal to yesterday’s close then:
        /// OBV(i) = OBV(i-1)
        /// </summary>
        /// <see cref="http://ta.mql4.com/indicators/volumes/on_balance_volume"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie obvSerie = new SingleDoubleSerie();
            obvSerie.Values.Add(OhlcList[0].Volume);

            for (int i = 1; i < OhlcList.Count; i++)
            {   
                double value = 0.0;
                if (OhlcList[i].Close > OhlcList[i - 1].Close)
                {
                    value = obvSerie.Values[i - 1].Value + OhlcList[i].Volume;
                }
                else if (OhlcList[i].Close < OhlcList[i - 1].Close)
                {
                    value = obvSerie.Values[i - 1].Value - OhlcList[i].Volume;
                }
                else if (OhlcList[i].Close == OhlcList[i - 1].Close)
                {
                    value = obvSerie.Values[i - 1].Value;
                }

                obvSerie.Values.Add(value);
            }

            return obvSerie;
        }
    }
}
