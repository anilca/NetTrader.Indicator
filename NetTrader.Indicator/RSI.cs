using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Relative Strength Index (RSI)
    /// </summary>
    public class RSI : IndicatorCalculatorBase<RSISerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period { get; set; }

        private List<double?> change = new List<double?>();

        public RSI(int period)
        {
            this.Period = period;
        }

        /// <summary>
        ///    RS = Average Gain / Average Loss
        ///    
        ///                  100
        ///    RSI = 100 - --------
        ///                 1 + RS
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi"/>
        /// <returns></returns>
        public override RSISerie Calculate()
        {
            RSISerie rsiSerie = new RSISerie();

            // Add null values for first item, iteration will start from second item of OhlcList
            rsiSerie.RS.Add(null);
            rsiSerie.RSI.Add(null);
            change.Add(null);

            for (int i = 1; i < OhlcList.Count; i++)
            {
                if (i >= this.Period)
                {
                    var averageGain = change.Where(x => x > 0).Sum() / change.Count;
                    var averageLoss = change.Where(x => x < 0).Sum() * (-1) / change.Count;
                    var rs = averageGain / averageLoss;
                    rsiSerie.RS.Add(rs);
                    var rsi = 100 - (100 / (1 + rs));
                    rsiSerie.RSI.Add(rsi);
                    // assign change for item
                    change.Add(OhlcList[i].Close - OhlcList[i - 1].Close);
                }
                else
                {   
                    rsiSerie.RS.Add(null);
                    rsiSerie.RSI.Add(null);
                    // assign change for item
                    change.Add(OhlcList[i].Close - OhlcList[i - 1].Close);
                }
            }

            return rsiSerie;
        }
    }
}
