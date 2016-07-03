using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class SAR : IndicatorCalculatorBase<SingleDoubleSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        protected double AccelerationFactor = 0.02;
        protected double MaximumAccelerationFactor = 0.2;

        public SAR()
        { 
        
        }

        public SAR(double accelerationFactor, double maximumAccelerationFactor)
        {
            this.AccelerationFactor = accelerationFactor;
            this.MaximumAccelerationFactor = maximumAccelerationFactor;
        }

        public override SingleDoubleSerie Calculate()
        {
            SingleDoubleSerie sarSerie = new SingleDoubleSerie();

            // Difference of High and Low
            List<double> differences = new List<double>();
            for (int i = 0; i < OhlcList.Count; i++)
            {
                double difference = OhlcList[i].High - OhlcList[i].Low;
                differences.Add(difference);
            }

            // STDEV of differences
            double stDev = Statistics.StandardDeviation(differences);

            double?[] sarArr = new double?[OhlcList.Count];

            double[] highList = OhlcList.Select(x => x.High).ToArray();
            double[] lowList = OhlcList.Select(x => x.Low).ToArray();

            /* Find first non-NA value */
            int beg = 1;
            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (OhlcList[i].High == 0 || OhlcList[i].Low == 0)
                {
                    sarArr[i] = 0;
                    beg++;
                }
                else
                {
                    break;
                }
            }
            // TODO: needs attention, understand better and replace variable names with meaningful ones.
            /* Initialize values needed by the routine */
            int sig0 = 1, sig1 = 0;
            double xpt0 = highList[beg - 1], xpt1 = 0;
            double af0 = AccelerationFactor, af1 = 0;
            double lmin, lmax;
            sarArr[beg - 1] = lowList[beg - 1] - stDev;

            for (int i = beg; i < OhlcList.Count; i++)
            {
                /* Increment signal, extreme point, and acceleration factor */
                sig1 = sig0;
                xpt1 = xpt0;
                af1 = af0;

                /* Local extrema */
                lmin = (lowList[i - 1] > lowList[i]) ? lowList[i] : lowList[i - 1];
                lmax = (highList[i - 1] > highList[i]) ? highList[i - 1] : highList[i];
                /* Create signal and extreme price vectors */
                if (sig1 == 1)
                {  /* Previous buy signal */
                    sig0 = (lowList[i] > sarArr[i - 1]) ? 1 : -1;  /* New signal */
                    xpt0 = (lmax > xpt1) ? lmax : xpt1;             /* New extreme price */
                }
                else
                {           /* Previous sell signal */
                    sig0 = (highList[i] < sarArr[i - 1]) ? -1 : 1;  /* New signal */
                    xpt0 = (lmin > xpt1) ? xpt1 : lmin;             /* New extreme price */
                }

                /*
                    * Calculate acceleration factor (af)
                    * and stop-and-reverse (sar) vector
                */

                /* No signal change */
                if (sig0 == sig1)
                {
                    /* Initial calculations */
                    sarArr[i] = sarArr[i - 1] + (xpt1 - sarArr[i - 1]) * af1;
                    af0 = (af1 == MaximumAccelerationFactor) ? MaximumAccelerationFactor : (AccelerationFactor + af1);
                    /* Current buy signal */
                    if (sig0 == 1)
                    {
                        af0 = (xpt0 > xpt1) ? af0 : af1;  /* Update acceleration factor */
                        sarArr[i] = (sarArr[i] > lmin) ? lmin : sarArr[i];  /* Determine sar value */
                    }
                    /* Current sell signal */
                    else
                    {
                        af0 = (xpt0 < xpt1) ? af0 : af1;  /* Update acceleration factor */
                        sarArr[i] = (sarArr[i] > lmax) ? sarArr[i] : lmax;   /* Determine sar value */
                    }
                }
                else /* New signal */
                {
                    af0 = AccelerationFactor;    /* reset acceleration factor */
                    sarArr[i] = xpt0;  /* set sar value */
                }
            }

            sarSerie.Values = sarArr.ToList();

            return sarSerie;
        }
    }
}
