using System.Collections.Generic;
using System.Linq;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Double Exponential Moving Average (DEMA)
    /// </summary>
    public class DEMA : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period { get; set; }
        protected ColumnType ColumnType { get; set; } = ColumnType.Close;

        public DEMA(int period, ColumnType columnType = ColumnType.Close)
        {
            this.Period = period;
            this.ColumnType = columnType;
        }

        /// <summary>
        /// DEMA = 2 * EMA - EMA of EMA
        /// </summary>
        /// <see cref="http://forex-indicators.net/trend-indicators/dema"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie demaSerie = new SingleDoubleSerie();
            EMA ema = new EMA(Period, false, ColumnType);
            ema.Load(OhlcList);
            List<double?> emaValues = ema.Calculate().Values;

            // assign EMA values to column
            for (int i = 0; i < OhlcList.Count; i++)
            {
                switch (ColumnType)
                {
                    case ColumnType.AdjClose:
                        OhlcList[i].AdjClose = emaValues[i] ?? 0.0;
                        break;
                    case ColumnType.Close:
                        OhlcList[i].Close = emaValues[i] ?? 0.0;
                        break;
                    case ColumnType.High:
                        OhlcList[i].High = emaValues[i] ?? 0.0;
                        break;
                    case ColumnType.Low:
                        OhlcList[i].Low = emaValues[i] ?? 0.0;
                        break;
                    case ColumnType.Open:
                        OhlcList[i].Open = emaValues[i] ?? 0.0;
                        break;
                    case ColumnType.Volume:
                        OhlcList[i].Volume = emaValues[i] ?? 0.0;
                        break;
                    default:
                        break;
                }
            }

            ema.Load(OhlcList.Skip(Period - 1).ToList());
            // EMA(EMA(value))
            List<double?> emaEmaValues = ema.Calculate().Values;
            for (int i = 0; i < Period - 1; i++)
            {
                emaEmaValues.Insert(0, null);
            }    

            // Calculate DEMA
            for (int i = 0; i < OhlcList.Count; i++) 
            {
                if (i >= 2 * Period - 2)
                {
                    var dema = 2 * emaValues[i] - emaEmaValues[i];
                    demaSerie.Values.Add(dema);
                }
                else
                {
                    demaSerie.Values.Add(null);
                }
            }

            return demaSerie;
        }
    }
}
