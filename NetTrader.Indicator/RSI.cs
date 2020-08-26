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

            for (int i = 1; i < OhlcList.Count; i++)
            {
                if (i > this.Period)
                {
                    int start = i - Period;
                    double gainSum = 0;
                    for (int j = start; j <= i; j++)
                    {
                        double thisChange = OhlcList[j].Close - OhlcList[j - 1].Close;
                        if (thisChange > 0)
                        {
                            gainSum += thisChange;
                        }
                    }
                    var averageGain = gainSum / Period;
                    double lossSum = 0;
                    for (int j = start; j <= i; j++)
                    {
                        double thisChange = OhlcList[j].Close - OhlcList[j - 1].Close;
                        if (thisChange < 0)
                        {
                            lossSum += thisChange;
                        }
                    }
                    var averageLoss = -1 * lossSum / Period;
                    var rs = averageGain / averageLoss;
                    rsiSerie.RS.Add(rs);
                    var rsi = 100 - (100 / (1 + rs));
                    rsiSerie.RSI.Add(rsi);
                }
                else
                {
                    rsiSerie.RS.Add(null);
                    rsiSerie.RSI.Add(null);
                }
            }
            return rsiSerie;
        }
    }
}
