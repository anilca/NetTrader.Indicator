using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Detrended Price Oscillator (DPO)
    /// </summary>
    public class DPO : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        public int Period = 10;

        public DPO()
        { 
        
        }

        public DPO(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// Price {X/2 + 1} periods ago less the X-period simple moving average.
        /// X refers to the number of periods used to calculate the Detrended Price 
        /// Oscillator. A 20-day DPO would use a 20-day SMA that is displaced by 11 
        /// periods {20/2 + 1 = 11}. This displacement shifts the 20-day SMA 11 days 
        /// to the left, which actually puts it in the middle of the look-back 
        /// period. The value of the 20-day SMA is then subtracted from the price 
        /// in the middle of this look-back period. In short, DPO(20) equals price
        /// 11 days ago less the 20-day SMA.  
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:detrended_price_osci"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie dpoSerie = new SingleDoubleSerie();

            SMA sma = new SMA(Period);
            sma.Load(OhlcList);
            List<double?> smaList = (sma.Calculate() as SingleDoubleSerie).Values;
                
            // shift to left (n / 2) + 1
            for (int i = 0; i < smaList.Count; i++)
            {   
                if (i >= Period - 1)
                {
                    smaList[i - ((Period / 2) + 1)] = smaList[i];
                    smaList[i] = null;
                }
            }

            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (smaList[i].HasValue)
                {
                    double dpo = OhlcList[i].Close - smaList[i].Value;
                    dpoSerie.Values.Add(dpo);
                }
                else
                {
                    dpoSerie.Values.Add(null);
                }
            }

            return dpoSerie;
        }
    }
}
