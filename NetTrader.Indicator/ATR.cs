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
        public int Period = 14;

        public ATR()
        { 
        
        }

        public ATR(int period)
        {
            this.Period = period;
        }

        /// <summary>
        /// TrueHigh = Highest of high[0] or close[-1]
        /// TrueLow = Highest of low[0] or close[-1]
        /// TR = TrueHigh - TrueLow
        /// ATR = EMA(TR)
        /// </summary>
        /// <see cref="http://www.fmlabs.com/reference/default.htm?url=TR.htm"/>
        /// <see cref="http://www.fmlabs.com/reference/default.htm?url=ATR.htm"/>
        /// <returns></returns>
        public override ATRSerie Calculate()
        {
            ATRSerie atrSerie = new ATRSerie();
            atrSerie.TrueHigh.Add(null);
            atrSerie.TrueLow.Add(null);
            atrSerie.TrueRange.Add(null);
            atrSerie.ATR.Add(null);

            for (int i = 1; i < OhlcList.Count; i++)
            {
                double trueHigh = OhlcList[i].High >= OhlcList[i - 1].Close ? OhlcList[i].High : OhlcList[i - 1].Close;
                atrSerie.TrueHigh.Add(trueHigh);
                double trueLow = OhlcList[i].Low <= OhlcList[i - 1].Close ? OhlcList[i].Low : OhlcList[i - 1].Close;
                atrSerie.TrueLow.Add(trueLow);
                double trueRange = trueHigh - trueLow;
                atrSerie.TrueRange.Add(trueRange);    
            }

            for (int i = 1; i < OhlcList.Count; i++)
            {
                OhlcList[i].Close = atrSerie.TrueRange[i].Value;
            }

            EMA ema = new EMA(Period, true);
            ema.Load(OhlcList.Skip(1).ToList());
            List<double?> atrList = ema.Calculate().Values;
            foreach (var atr in atrList)
            {
                atrSerie.ATR.Add(atr);
            }

            return atrSerie;
        }
    }
}
