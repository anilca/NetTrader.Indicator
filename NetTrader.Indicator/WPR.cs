using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// William %R
    /// </summary>
    public class WPR : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }

        public int Period = 14;

        public WPR()
        { 
        
        }

        public WPR(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// %R = (Highest High - Close)/(Highest High - Lowest Low) * 100
        /// Lowest Low = lowest low for the look-back period
        /// Highest High = highest high for the look-back period
        /// %R is multiplied by -100 correct the inversion and move the decimal.
        /// </summary>
        /// <see cref="http://www.fmlabs.com/reference/default.htm?url=WilliamsR.htm"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie wprSerie = new SingleDoubleSerie();

            for (int i = 0; i < OhlcList.Count; i++)
            {   
                if (i >= Period - 1)
                {
                    double highestHigh = HighestHigh(i);
                    double lowestLow = LowestLow(i);
                    double wpr = (highestHigh - OhlcList[i].Close) / (highestHigh - lowestLow) * (100);
                    wprSerie.Values.Add(wpr);
                }
                else
                {
                    wprSerie.Values.Add(null);
                }
            }

            return wprSerie;
        }

        private double HighestHigh(int index)
        {
            int startIndex = index - (Period - 1);
            int endIndex = index;

            double highestHigh = 0.0;
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (OhlcList[i].High > highestHigh)
                {
                    highestHigh = OhlcList[i].High;    
                }
            }

            return highestHigh;
        }

        private double LowestLow(int index)
        {
            int startIndex = index - (Period - 1);
            int endIndex = index;

            double lowestLow = double.MaxValue;
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (OhlcList[i].Low < lowestLow)
                {
                    lowestLow = OhlcList[i].Low;
                }
            }

            return lowestLow;
        }
    }
}
