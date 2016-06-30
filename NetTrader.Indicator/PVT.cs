using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Price Volume Trend (PVT)
    /// </summary>
    public class PVT : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }

        /// <summary>
        /// PVT = [((CurrentClose - PreviousClose) / PreviousClose) x Volume] + PreviousPVT
        /// </summary>
        /// <see cref="https://www.tradingview.com/stock-charts-support/index.php/Price_Volume_Trend_(PVT)"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie pvtSerie = new SingleDoubleSerie();
            pvtSerie.Values.Add(null);    

            for (int i = 1; i < OhlcList.Count; i++)
            {
                pvtSerie.Values.Add((((OhlcList[i].Close - OhlcList[i - 1].Close) / OhlcList[i - 1].Close) * OhlcList[i].Volume) + pvtSerie.Values[i - 1]);
            }

            return pvtSerie;
        }
    }
}
