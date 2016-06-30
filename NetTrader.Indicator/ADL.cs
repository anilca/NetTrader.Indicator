using NetTrader.Indicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Accumulation / Distribution Line 
    /// </summary>
    public class ADL : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }

        /// <summary>
        /// Acc/Dist = ((Close – Low) – (High – Close)) / (High – Low) * Period's volume
        /// </summary>
        /// <see cref="http://www.investopedia.com/terms/a/accumulationdistribution.asp"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie adlSerie = new SingleDoubleSerie();
            foreach (var ohlc in OhlcList)
            {
                double value = ((ohlc.Close - ohlc.Low) - (ohlc.High - ohlc.Close)) / (ohlc.High - ohlc.Low) * ohlc.Volume;
                adlSerie.Values.Add(value);
            }

            return adlSerie;
        }
    }
}
