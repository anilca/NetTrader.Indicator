using System.Collections.Generic;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Exponential Moving Average
    /// </summary>
    public class EMA : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period = 10;
        protected bool Wilder = false;
        protected ColumnType ColumnType { get; set; } = ColumnType.Close;

        public EMA()
        { 
        
        }

        public EMA(int period, bool wilder, ColumnType columnType = ColumnType.Close)
        {
            this.Period = period;
            this.Wilder = wilder;
            this.ColumnType = columnType;
        }

        /// <summary>
        /// SMA: 10 period sum / 10 
        /// Multiplier: (2 / (Time periods + 1) ) = (2 / (10 + 1) ) = 0.1818 (18.18%)
        /// EMA: {Close - EMA(previous day)} x multiplier + EMA(previous day). 
        /// for Wilder parameter details: http://www.inside-r.org/packages/cran/TTR/docs/GD
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:moving_averages"/>
        /// <returns></returns>
        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie emaSerie = new SingleDoubleSerie();
            var multiplier = !this.Wilder ? (2.0 / (double)(Period + 1)) : (1.0 / (double)Period);

            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (i >= Period - 1)
                {       
                    double value = 0.0;
                    switch (ColumnType)
                    {
                        case ColumnType.AdjClose:
                            value = OhlcList[i].AdjClose;
                            break;
                        case ColumnType.Close:
                            value = OhlcList[i].Close;
                            break;
                        case ColumnType.High:
                            value = OhlcList[i].High;
                            break;
                        case ColumnType.Low:
                            value = OhlcList[i].Low;
                            break;
                        case ColumnType.Open:
                            value = OhlcList[i].Open;
                            break;
                        case ColumnType.Volume:
                            value = OhlcList[i].Volume;
                            break;
                        default:
                            break;
                    }
                    
                    if (emaSerie.Values[i - 1].HasValue)
                    {
                        var emaPrev = emaSerie.Values[i - 1].Value;
                        var ema = (value - emaPrev) * multiplier + emaPrev;
                        emaSerie.Values.Add(ema);
                    }
                    else
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
                        var ema = sum / Period;
                        emaSerie.Values.Add(ema);
                    }
                }
                else
                {
                    emaSerie.Values.Add(null);
                }
            }

            return emaSerie;
        }
    }
}
