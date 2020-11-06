using System.Collections.Generic;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Weighted Moving Average
    /// </summary>
    public class WMA : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period { get; set; }
        protected ColumnType ColumnType { get; set; } = ColumnType.Close;

        public WMA(int period, ColumnType columnType = ColumnType.Close)
        {
            this.Period = period;
            this.ColumnType = columnType;
        }

        /// <summary>
        /// Therefore the 5 Day WMA is 83(5/15) + 81(4/15) + 79(3/15) + 79(2/15) + 77(1/15) = 80.7
        /// Day	     1	2	3	4	5 (current)
        /// Price	77	79	79	81	83
        /// WMA	 	 	 	 	    80.7
        /// </summary>
        /// <see cref="http://fxtrade.oanda.com/learn/forex-indicators/weighted-moving-average"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie wmaSerie = new SingleDoubleSerie();

            int weightSum = 0;
            for (int i = 1; i <= Period; i++)
            {
                weightSum += i;
            }

            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (i >= Period - 1)
                {
                    double wma = 0.0;
                    int weight = 1;
                    for (int j = i - (Period - 1); j <= i; j++)
                    {
                        switch (ColumnType)
                        {
                            case ColumnType.AdjClose:
                                wma += ((double)weight / weightSum) * OhlcList[j].AdjClose;
                                break;
                            case ColumnType.Close:
                                wma += ((double)weight / weightSum) * OhlcList[j].Close;
                                break;
                            case ColumnType.High:
                                wma += ((double)weight / weightSum) * OhlcList[j].High;
                                break;
                            case ColumnType.Low:
                                wma += ((double)weight / weightSum) * OhlcList[j].Low;
                                break;
                            case ColumnType.Open:
                                wma += ((double)weight / weightSum) * OhlcList[j].Open;
                                break;
                            case ColumnType.Volume:
                                wma += ((double)weight / weightSum) * OhlcList[j].Volume;
                                break;
                            default:
                                break;
                        }

                        weight++;
                    }
                    wmaSerie.Values.Add(wma);
                }
                else
                {
                    wmaSerie.Values.Add(null);
                }
            }

            return wmaSerie;
        }
    }
}
