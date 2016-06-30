using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Commodity Channel Index (CCI)
    /// </summary>
    public class CCI : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        public int Period = 20;
        public double Factor = 0.015;

        public CCI()
        { 
        
        }

        public CCI(int period, int factor)
        {
            this.Period = period;
            this.Factor = factor;
        }

        /// <summary>
        /// Commodity Channel Index (CCI)
        /// tp = (high + low + close) / 3
        /// cci = (tp - SMA(tp)) / (Factor * meanDeviation(tp))
        /// </summary>
        /// <see cref="http://www.fmlabs.com/reference/default.htm?url=CCI.htm"/>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:commodity_channel_index_cci"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie cciSerie = new SingleDoubleSerie();
                
            for (int i = 0; i < OhlcList.Count; i++)
            {
                OhlcList[i].Close = (OhlcList[i].High + OhlcList[i].Low + OhlcList[i].Close) / 3;
            }

            SMA sma = new SMA(Period);
            sma.Load(OhlcList);
            List<double?> smaList = (sma.Calculate() as SingleDoubleSerie).Values;

            List<double?> meanDeviationList = new List<double?>();
            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (i >= Period - 1)
                {
                    double total = 0.0;
                    for (int j = i; j >= i - (Period - 1); j--)
                    {
                        total += Math.Abs(smaList[i].Value - OhlcList[j].Close);
                    }
                    meanDeviationList.Add(total / (double)Period);
                    
                    double cci = (OhlcList[i].Close - smaList[i].Value) / (Factor * meanDeviationList[i].Value);
                    cciSerie.Values.Add(cci);
                }
                else
                {
                    meanDeviationList.Add(null);
                    cciSerie.Values.Add(null);
                }
            }

            return cciSerie;
        }
    }
}
