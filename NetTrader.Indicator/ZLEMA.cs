using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class ZLEMA : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }

        public int Period = 10;

        public ZLEMA()
        { 
        
        }

        public ZLEMA(int period)
        {
            this.Period = period;
        }

        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie zlemaSerie = new SingleDoubleSerie();

            double ratio = 2.0 / (double)(Period + 1);
            double lag = 1 / ratio;
            double wt = lag - ((int)lag / 1.0) * 1.0; //DMOD( lag, 1.0D0 )
            double meanOfFirstPeriod = OhlcList.Take(Period).Select(x => x.Close).Sum() / Period; 
            
            for (int i = 0; i < OhlcList.Count; i++)
            {   
                if (i > Period - 1)
                {
                    int loc = (int)(i - lag);
                    double zlema = ratio * (2 * OhlcList[i].Close - (OhlcList[loc].Close * (1 - wt) + OhlcList[loc + 1].Close * wt)) + (1 - ratio) * zlemaSerie.Values[i - 1].Value;
                    zlemaSerie.Values.Add(zlema);
                }
                else if (i == Period - 1)
	            {
                    zlemaSerie.Values.Add(meanOfFirstPeriod);
	            }
                else
                {
                    zlemaSerie.Values.Add(null);
                }
            }

            return zlemaSerie;
        }
    }
}
