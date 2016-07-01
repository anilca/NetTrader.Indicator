using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Rate of Change (ROC)
    /// </summary>
    public class ROC : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period { get; set; }

        public ROC(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// ROC = [(Close - Close n periods ago) / (Close n periods ago)] * 100
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:rate_of_change_roc_and_momentum"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie rocSerie = new SingleDoubleSerie();
                
            for (int i = 0; i < OhlcList.Count; i++)
            {   
                if (i >= this.Period)
                {
                    rocSerie.Values.Add(((OhlcList[i].Close - OhlcList[i - this.Period].Close) / OhlcList[i - this.Period].Close) * 100);
                }
                else
                {
                    rocSerie.Values.Add(null);
                }
            }

            return rocSerie;
        }
    }
}
