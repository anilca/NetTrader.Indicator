using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Chaikin Money Flow
    /// </summary>
    public class CMF : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period = 20;

        public CMF()
        { 
        
        }

        public CMF(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// Chaikin Money Flow
        /// Money Flow Multiplier = [(Close  -  Low) - (High - Close)] /(High - Low) 
        /// Money Flow Volume = Money Flow Multiplier x Volume for the Period
        /// 20-period CMF = 20-period Sum of Money Flow Volume / 20 period Sum of Volume
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:chaikin_money_flow_cmf"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie cmfSerie = new SingleDoubleSerie();

            List<double> moneyFlowVolumeList = new List<double>();

            for (int i = 0; i < OhlcList.Count; i++)
            {
                double moneyFlowMultiplier = ((OhlcList[i].Close - OhlcList[i].Low) - (OhlcList[i].High - OhlcList[i].Close)) / (OhlcList[i].High - OhlcList[i].Low);
                
                moneyFlowVolumeList.Add(moneyFlowMultiplier * OhlcList[i].Volume);

                if (i >= Period - 1)
                {
                    double sumOfMoneyFlowVolume = 0.0, sumOfVolume = 0.0;
                    for (int j = i; j >= i - (Period - 1); j--)
                    {
                        sumOfMoneyFlowVolume += moneyFlowVolumeList[j];
                        sumOfVolume += OhlcList[j].Volume;
                    }
                    cmfSerie.Values.Add(sumOfMoneyFlowVolume / sumOfVolume);
                }
                else
                {
                    cmfSerie.Values.Add(null);
                }
            }

            return cmfSerie;
        }
    }
}
