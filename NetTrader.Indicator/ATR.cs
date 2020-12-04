using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// True Range / Average True Range
    /// </summary>
    public class ATR : IndicatorCalculatorBase<ATRSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period = 14;

        public ATR()
        { 
        
        }

        public ATR(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// TR = Maximum of the following 3 calculations
        /// Method 1: Current High less the current Low
        /// Method 2: Current High less the previous Close (absolute value)
        /// Method 3: Current Low less the previous Close (absolute value)
        /// ATR = [(Prior ATR x (Period -1)) + Current TR] / Period
        /// </summary>
        /// <see cref="https://school.stockcharts.com/doku.php?id=technical_indicators:average_true_range_atr"/>
        /// <returns></returns>
        public override ATRSerie Calculate()
        {
            ATRSerie atrSerie = new ATRSerie();

            for (int i = 0; i < OhlcList.Count; i++)
            {
                List<double> trueRangeList = new List<double>(3)
                {
                    OhlcList[i].High - OhlcList[i].Low
                };
                if (i > 0)
                {
                    trueRangeList.Add(Math.Abs(OhlcList[i].High - OhlcList[i - 1].Close));
                    trueRangeList.Add(Math.Abs(OhlcList[i].Low - OhlcList[i - 1].Close));
                }
                var currentTrueRange = trueRangeList.Max();
                atrSerie.TrueRange.Add(currentTrueRange);
                if (i == Period - 1)
                {
                    atrSerie.ATR.Add(atrSerie.TrueRange.Average());
                }
                else if (i > Period - 1)
                {
                    var currentAtr = ((atrSerie.ATR.Last() * (Period - 1)) + currentTrueRange) / Period;
                    atrSerie.ATR.Add(currentAtr);
                }
                else
                {
                    atrSerie.ATR.Add(null);
                }
            }

            return atrSerie;
        }
    }
}
