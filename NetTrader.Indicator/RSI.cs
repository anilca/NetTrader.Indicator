using System.Collections.Generic;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Relative Strength Index (RSI)
    /// </summary>
    public class RSI : IndicatorCalculatorBase<RSISerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period { get; set; }

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

            double gainSum = 0;
            double lossSum = 0;
            for (int i = 1; i < Period; i++)
            {
                double thisChange = OhlcList[i].Close - OhlcList[i - 1].Close;
                if (thisChange > 0)
                {
                    gainSum += thisChange;
                }
                else
                {
                    lossSum += (-1) * thisChange;
                }
                rsiSerie.RS.Add(null);
                rsiSerie.RSI.Add(null);
            }
            
            var averageGain = gainSum / Period;
            var averageLoss = lossSum / Period;
            var rs = averageGain / averageLoss;
            rsiSerie.RS.Add(rs);
            var rsi = 100 - (100 / (1 + rs));
            rsiSerie.RSI.Add(rsi);

            for (int i = Period + 1; i < OhlcList.Count; i++)
            {
                double thisChange = OhlcList[i].Close - OhlcList[i - 1].Close;
                if (thisChange > 0)
                {
                    averageGain = (averageGain * (Period - 1) + thisChange) / Period;
                    averageLoss = (averageLoss * (Period - 1)) / Period;
                }
                else
                {
                    averageGain = (averageGain * (Period - 1)) / Period;
                    averageLoss = (averageLoss * (Period - 1) + (-1) * thisChange) / Period;
                }
                rs = averageGain / averageLoss;
                rsiSerie.RS.Add(rs);
                rsi = 100 - (100 / (1 + rs));
                rsiSerie.RSI.Add(rsi);
            }

            return rsiSerie;
        }
    }
}
