using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Chande Momentum Oscillator (CMO)
    /// </summary>
    public class CMO : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period = 14;

        public CMO() 
        { 
        
        }

        public CMO(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// Chande Momentum Oscillator (CMO)
        /// </summary>
        /// <see cref="http://www.fmlabs.com/reference/default.htm?url=CMO.htm"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie cmoSerie = new SingleDoubleSerie();
            cmoSerie.Values.Add(null);

            List<double> upValues = new List<double>();
            upValues.Add(0);
            List<double> downValues = new List<double>();
            downValues.Add(0);

            for (int i = 1; i < OhlcList.Count; i++)
            {
                if (OhlcList[i].Close > OhlcList[i - 1].Close)
                {
                    upValues.Add(OhlcList[i].Close - OhlcList[i - 1].Close);
                    downValues.Add(0);        
                }
                else if (OhlcList[i].Close < OhlcList[i - 1].Close)
                {
                    upValues.Add(0);
                    downValues.Add(OhlcList[i - 1].Close - OhlcList[i].Close);
                }
                else
                {
                    upValues.Add(0);
                    downValues.Add(0);
                }

                if (i >= Period)
                {
                    double upTotal = 0.0, downTotal = 0.0;
                    for (int j = i; j >= i - (Period - 1); j--)
                    {
                        upTotal += upValues[j];
                        downTotal += downValues[j];
                    }

                    double cmo = 100 * (upTotal - downTotal) / (upTotal + downTotal);
                    cmoSerie.Values.Add(cmo);
                }
                else
                {
                    cmoSerie.Values.Add(null);
                }
            }

            return cmoSerie;
        }
    }
}
