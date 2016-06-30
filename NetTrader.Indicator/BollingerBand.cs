using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Bollinger Bands
    /// </summary>
    public class BollingerBand : IndicatorCalculatorBase<BollingerBandSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        public int Period = 20;
        public int Factor = 2;

        public BollingerBand()
        { 
        
        }

        public BollingerBand(int period, int factor)
        {
            this.Period = period;
            this.Factor = factor;
        }

        /// <summary>
        /// tp = (high + low + close) / 3
        /// MidBand = SMA(TP)
        /// UpperBand = MidBand + Factor * Stdev(tp)
        /// LowerBand = MidBand - Factor * Stdev(tp)
        /// BandWidth = (UpperBand - LowerBand) / MidBand
        /// </summary>
        /// <see cref="http://www.fmlabs.com/reference/default.htm?url=Bollinger.htm"/>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:bollinger_bands"/>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:bollinger_band_width"/>
        /// <returns></returns>
        public override BollingerBandSerie Calculate()
        {
            BollingerBandSerie bollingerBandSerie = new BollingerBandSerie();   

            double totalAverage = 0;
            double totalSquares = 0;

            for (int i = 0; i < OhlcList.Count; i++)
            {
                OhlcList[i].Close = (OhlcList[i].High + OhlcList[i].Low + OhlcList[i].Close) / 3;

                totalAverage += OhlcList[i].Close;
                totalSquares += Math.Pow(OhlcList[i].Close, 2);

                if (i >= Period - 1)
                {
                    double average = totalAverage / Period;
                    double stdev = Math.Sqrt((totalSquares - Math.Pow(totalAverage, 2) / Period) / Period);

                    bollingerBandSerie.MidBand.Add(average);
                    double up = average + Factor * stdev;
                    bollingerBandSerie.UpperBand.Add(up);
                    double down = average - Factor * stdev;
                    bollingerBandSerie.LowerBand.Add(down);
                    double bandWidth = (up - down) / average;
                    bollingerBandSerie.BandWidth.Add(bandWidth);
                    double bPercent = (OhlcList[i].Close - down) / (up - down);
                    bollingerBandSerie.BPercent.Add(bPercent);

                    totalAverage -= OhlcList[i - Period + 1].Close;
                    totalSquares -= Math.Pow(OhlcList[i - Period + 1].Close, 2);
                }
                else
                {
                    bollingerBandSerie.MidBand.Add(null);
                    bollingerBandSerie.UpperBand.Add(null);
                    bollingerBandSerie.LowerBand.Add(null);
                    bollingerBandSerie.BandWidth.Add(null);
                    bollingerBandSerie.BPercent.Add(null);
                }
            }

            return bollingerBandSerie;
        }
    }
}
