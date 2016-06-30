using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class ADX : IndicatorCalculatorBase<ADXSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        public int Period = 14;

        public ADX()
        { 
        
        }

        public ADX(int period)
        {
            this.Period = period;
        }

        public override ADXSerie Calculate()
        {
            ADXSerie adxSerie = new ADXSerie();
                
            List<Ohlc> tempOhlcList = new List<Ohlc>();
            for (int i = 0; i < OhlcList.Count; i++)
            {
                Ohlc tempOhlc = new Ohlc() { Close = OhlcList[i].High };
                tempOhlcList.Add(tempOhlc);
            }
            Momentum momentum = new Momentum();
            momentum.Load(tempOhlcList);
            List<double?> highMomentums = (momentum.Calculate() as SingleDoubleSerie).Values;

            tempOhlcList = new List<Ohlc>();
            for (int i = 0; i < OhlcList.Count; i++)
            {
                Ohlc tempOhlc = new Ohlc() { Close = OhlcList[i].Low };
                tempOhlcList.Add(tempOhlc);
            }   
            momentum = new Momentum();
            momentum.Load(tempOhlcList);
            List<double?> lowMomentums = (momentum.Calculate() as SingleDoubleSerie).Values;
            for (int i = 0; i < lowMomentums.Count; i++)
            {
                if (lowMomentums[i].HasValue)
                {
                    lowMomentums[i] *= -1;
                }
            }

            //DMIp <- ifelse( dH==dL | (dH< 0 & dL< 0), 0, ifelse( dH >dL, dH, 0 ) )
            List<double?> DMIPositives = new List<double?>() { null };
            // DMIn <- ifelse( dH==dL | (dH< 0 & dL< 0), 0, ifelse( dH <dL, dL, 0 ) )
            List<double?> DMINegatives = new List<double?>() { null };
            for (int i = 1; i < OhlcList.Count; i++)
            {
                if (highMomentums[i] == lowMomentums[i] || (highMomentums[i] < 0 & lowMomentums[i] < 0))
                {
                    DMIPositives.Add(0);
                }
                else
                {
                    if (highMomentums[i] > lowMomentums[i])
                    {
                        DMIPositives.Add(highMomentums[i]);
                    }
                    else
                    {
                        DMIPositives.Add(0);
                    }
                }

                if (highMomentums[i] == lowMomentums[i] || (highMomentums[i] < 0 & lowMomentums[i] < 0))
                {
                    DMINegatives.Add(0);
                }
                else
                {
                    if (highMomentums[i] < lowMomentums[i])
                    {
                        DMINegatives.Add(lowMomentums[i]);
                    }
                    else
                    {
                        DMINegatives.Add(0);
                    }
                }
            }

            ATR atr = new ATR();
            atr.Load(OhlcList);
            List<double?> trueRanges = (atr.Calculate() as ATRSerie).TrueRange;
            adxSerie.TrueRange = trueRanges;

            List<double?> trSum = wilderSum(trueRanges);

            // DIp <- 100 * wilderSum(DMIp, n=n) / TRsum
            List<double?> DIPositives = new List<double?>();
            List<double?> wilderSumOfDMIp = wilderSum(DMIPositives);
            for (int i = 0; i < wilderSumOfDMIp.Count; i++)
            {
                if (wilderSumOfDMIp[i].HasValue)
                {
                    DIPositives.Add(wilderSumOfDMIp[i].Value * 100 / trSum[i].Value);
                }
                else
                {
                    DIPositives.Add(null);
                }
            }
            adxSerie.DIPositive = DIPositives;

            // DIn <- 100 * wilderSum(DMIn, n=n) / TRsum
            List<double?> DINegatives = new List<double?>();
            List<double?> wilderSumOfDMIn = wilderSum(DMINegatives);
            for (int i = 0; i < wilderSumOfDMIn.Count; i++)
            {
                if (wilderSumOfDMIn[i].HasValue)
                {
                    DINegatives.Add(wilderSumOfDMIn[i].Value * 100 / trSum[i].Value);
                }
                else
                {
                    DINegatives.Add(null);
                }
            }
            adxSerie.DINegative = DINegatives;

            // DX  <- 100 * ( abs(DIp - DIn) / (DIp + DIn) )
            List<double?> DX = new List<double?>();
            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (DIPositives[i].HasValue)
                {
                    double? dx = 100 * (Math.Abs(DIPositives[i].Value - DINegatives[i].Value) / (DIPositives[i].Value + DINegatives[i].Value));
                    DX.Add(dx);
                }
                else
	            {
                    DX.Add(null);
	            }
            }
            adxSerie.DX = DX;
                
            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (DX[i].HasValue)
                {
                    OhlcList[i].Close = DX[i].Value;
                }
                else
                {
                    OhlcList[i].Close = 0.0;
                }
            }

            EMA ema = new EMA(Period, true);
            ema.Load(OhlcList.Skip(Period).ToList());
            List<double?> emaValues = (ema.Calculate() as SingleDoubleSerie).Values;
            for (int i = 0; i < Period; i++)
            {
                emaValues.Insert(0, null);
            }
            adxSerie.ADX = emaValues;

            return adxSerie;
        }

        private List<double?> wilderSum(List<double?> values)
        {
            double?[] wilderSumsArray = new double?[values.Count];
            double?[] valueArr = values.ToArray();

            int beg = Period - 1;
            double sum = 0;
            int i = 0;
            for (i = 0; i < beg; i++)
            {
                /* Account for leading NAs in input */
                if (!valueArr[i].HasValue)
                {
                    wilderSumsArray[i] = null;
                    beg++;
                    wilderSumsArray[beg] = 0;
                    continue;
                }
                /* Set leading NAs in output */
                if (i < beg)
                {
                    wilderSumsArray[i] = null;
                }
                /* Calculate raw sum to start */
                sum += valueArr[i].Value;
            }

            wilderSumsArray[beg] = valueArr[i] + sum * (Period - 1) / Period;

            /* Loop over non-NA input values */
            for (i = beg + 1; i < values.Count; i++)
            {
                wilderSumsArray[i] = valueArr[i] + wilderSumsArray[i - 1] * (Period - 1) / Period;
            }

            return wilderSumsArray.ToList();
        }
    }
}
