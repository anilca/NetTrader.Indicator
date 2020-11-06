using System.Collections.Generic;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Simple Moving Average
    /// </summary>
    public class SMA : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period { get; set; }
        protected ColumnType ColumnType { get; set; } = ColumnType.Close;

        public SMA(int period, ColumnType columnType = ColumnType.Close)
        {
            this.Period = period;
            this.ColumnType = columnType;
        }

        /// <summary>
        /// Daily Closing Prices: 11,12,13,14,15,16,17 
        /// First day of 5-day SMA: (11 + 12 + 13 + 14 + 15) / 5 = 13
        /// Second day of 5-day SMA: (12 + 13 + 14 + 15 + 16) / 5 = 14
        /// Third day of 5-day SMA: (13 + 14 + 15 + 16 + 17) / 5 = 15 
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:moving_averages"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie smaSerie = new SingleDoubleSerie();
                
            for (int i = 0; i < OhlcList.Count; i++)
            {   
                if (i >= Period - 1)
                {
                    double sum = 0;
                    for (int j = i; j >= i - (Period - 1); j--)
                    {
                        switch (ColumnType)
                        {
                            case ColumnType.AdjClose:
                                sum += OhlcList[j].AdjClose;
                                break;
                            case ColumnType.Close:
                                sum += OhlcList[j].Close;
                                break;
                            case ColumnType.High:
                                sum += OhlcList[j].High;
                                break;
                            case ColumnType.Low:
                                sum += OhlcList[j].Low;
                                break;
                            case ColumnType.Open:
                                sum += OhlcList[j].Open;
                                break;
                            case ColumnType.Volume:
                                sum += OhlcList[j].Volume;
                                break;
                            default:
                                break;
                        }
                    }
                    double avg = sum / Period;
                    smaSerie.Values.Add(avg);
                }
                else
                {
                    smaSerie.Values.Add(null);
                }
            }

            return smaSerie;
        }
    }
}
