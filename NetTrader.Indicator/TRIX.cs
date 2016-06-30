using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Triple Smoothed Exponential Oscillator
    /// </summary>
    public class TRIX : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }

        public int Period = 20;
        public bool CalculatePercentage = true;

        public TRIX()
        {

        }

        public TRIX(int period, bool calculatePercentage)
        {
            this.Period = period;
            this.CalculatePercentage = calculatePercentage;
        }

        /// <summary>
        /// 1 - EMA of Close prices [EMA(Close)]
        /// 2 - Double smooth [EMA(EMA(Close))]
        /// 3 - Triple smooth [EMA(EMA(EMA(Close)))]
        /// 4 - a) Calculation with percentage: [ROC(EMA(EMA(EMA(Close))))]
        /// 4 - b) Calculation with percentage: [Momentum(EMA(EMA(EMA(Close))))]
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:trix"/>
        /// <see cref="http://www.fmlabs.com/reference/default.htm?url=TRIX.htm"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            // EMA calculation
            EMA ema = new EMA(Period, false);
            ema.Load(OhlcList);
            List<double?> emaValues = (ema.Calculate() as SingleDoubleSerie).Values;
            for (int i = 0; i < OhlcList.Count; i++)
            {
                OhlcList[i].Close = emaValues[i].HasValue ? emaValues[i].Value : 0.0;
            }

            // Double smooth
            ema.Load(OhlcList.Skip(Period - 1).ToList());
            List<double?> doubleSmoothValues = (ema.Calculate() as SingleDoubleSerie).Values;
            for (int i = 0; i < Period - 1; i++)
            {
                doubleSmoothValues.Insert(0, null);
            }
            for (int i = 0; i < OhlcList.Count; i++)
            {
                OhlcList[i].Close = doubleSmoothValues[i].HasValue ? doubleSmoothValues[i].Value : 0.0;
            }

            // Triple smooth
            ema.Load(OhlcList.Skip(2 * (Period - 1)).ToList());
            List<double?> tripleSmoothValues = (ema.Calculate() as SingleDoubleSerie).Values;
            for (int i = 0; i < (2 * (Period - 1)); i++)
            {
                tripleSmoothValues.Insert(0, null);
            }
            for (int i = 0; i < OhlcList.Count; i++)
            {
                OhlcList[i].Close = tripleSmoothValues[i].HasValue ? tripleSmoothValues[i].Value : 0.0;
            }

            // Last step
            SingleDoubleSerie trixSerie = new SingleDoubleSerie();

            if (CalculatePercentage)
            {
                ROC roc = new ROC(1);
                roc.Load(OhlcList.Skip(3 * (Period - 1)).ToList());
                trixSerie = (roc.Calculate() as SingleDoubleSerie);
            }
            else
            {
                Momentum momentum = new Momentum();
                momentum.Load(OhlcList.Skip(3 * (Period - 1)).ToList());
                trixSerie = (momentum.Calculate() as SingleDoubleSerie);
            }

            for (int i = 0; i < (3 * (Period - 1)); i++)
            {
                trixSerie.Values.Insert(0, null);
            }

            return trixSerie;
        }
    }
}
