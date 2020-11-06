using System.Collections.Generic;
using System.Linq;

namespace NetTrader.Indicator
{
    public class ZLEMA : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected int Period = 10;
        protected ColumnType ColumnType { get; set; } = ColumnType.Close;

        public ZLEMA()
        { 
        
        }

        public ZLEMA(int period, ColumnType columnType = ColumnType.Close)
        {
            this.Period = period;
            this.ColumnType = columnType;
        }

        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie zlemaSerie = new SingleDoubleSerie();

            double ratio = 2.0 / (double)(Period + 1);
            double lag = 1 / ratio;
            double wt = lag - ((int)lag / 1.0) * 1.0; //DMOD( lag, 1.0D0 )
            double meanOfFirstPeriod = 0.0;
            switch (ColumnType)
            {
                case ColumnType.AdjClose:
                    meanOfFirstPeriod = OhlcList.Take(Period).Select(x => x.AdjClose).Sum() / Period;
                    break;
                case ColumnType.Close:
                    meanOfFirstPeriod = OhlcList.Take(Period).Select(x => x.Close).Sum() / Period;
                    break;
                case ColumnType.High:
                    meanOfFirstPeriod = OhlcList.Take(Period).Select(x => x.High).Sum() / Period;
                    break;
                case ColumnType.Low:
                    meanOfFirstPeriod = OhlcList.Take(Period).Select(x => x.Low).Sum() / Period;
                    break;
                case ColumnType.Open:
                    meanOfFirstPeriod = OhlcList.Take(Period).Select(x => x.Open).Sum() / Period;
                    break;
                case ColumnType.Volume:
                    meanOfFirstPeriod = OhlcList.Take(Period).Select(x => x.Volume).Sum() / Period;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < OhlcList.Count; i++)
            {   
                if (i > Period - 1)
                {
                    int loc = (int)(i - lag);

                    double zlema = 0.0;
                    switch (ColumnType)
                    {
                        case ColumnType.AdjClose:
                            zlema = ratio * (2 * OhlcList[i].AdjClose - (OhlcList[loc].AdjClose * (1 - wt) + OhlcList[loc + 1].AdjClose * wt)) + (1 - ratio) * zlemaSerie.Values[i - 1].Value;
                            break;
                        case ColumnType.Close:
                            zlema = ratio * (2 * OhlcList[i].Close - (OhlcList[loc].Close * (1 - wt) + OhlcList[loc + 1].Close * wt)) + (1 - ratio) * zlemaSerie.Values[i - 1].Value;
                            break;
                        case ColumnType.High:
                            zlema = ratio * (2 * OhlcList[i].High - (OhlcList[loc].High * (1 - wt) + OhlcList[loc + 1].High * wt)) + (1 - ratio) * zlemaSerie.Values[i - 1].Value;
                            break;
                        case ColumnType.Low:
                            zlema = ratio * (2 * OhlcList[i].Low - (OhlcList[loc].Low * (1 - wt) + OhlcList[loc + 1].Low * wt)) + (1 - ratio) * zlemaSerie.Values[i - 1].Value;
                            break;
                        case ColumnType.Open:
                            zlema = ratio * (2 * OhlcList[i].Open - (OhlcList[loc].Open * (1 - wt) + OhlcList[loc + 1].Open * wt)) + (1 - ratio) * zlemaSerie.Values[i - 1].Value;
                            break;
                        case ColumnType.Volume:
                            zlema = ratio * (2 * OhlcList[i].Volume - (OhlcList[loc].Volume * (1 - wt) + OhlcList[loc + 1].Volume * wt)) + (1 - ratio) * zlemaSerie.Values[i - 1].Value;
                            break;
                        default:
                            break;
                    }

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
