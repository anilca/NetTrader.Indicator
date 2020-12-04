using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

namespace NetTrader.Indicator.Test
{
    public class IndicatorTests
    {
        private readonly string csvPath = Directory.GetCurrentDirectory() + "\\table.csv";

        [Fact]
        public void IndicatorsShouldNotChangeTheSourceOhlcList()
        {
            // read ohlc list from csv file
            List<Ohlc> ohlcList = ReadCsvFile(csvPath);
            // create a deep clone of the list
            List<Ohlc> ohlcListDeepClone = ohlcList.DeepClone();
            // run MACD on the ohlcList
            MACD macd = new MACD(true);
            macd.Load(ohlcList);
            MACDSerie serie = macd.Calculate();
            // check if the ohlcList is still the same as its deep copy
            int i = 0;
            foreach (var item in ohlcList)
            {
                Assert.Equal(item.AdjClose, ohlcListDeepClone[i].AdjClose);
                Assert.Equal(item.Close, ohlcListDeepClone[i].Close);
                Assert.Equal(item.Date, ohlcListDeepClone[i].Date);
                Assert.Equal(item.High, ohlcListDeepClone[i].High);
                Assert.Equal(item.Low, ohlcListDeepClone[i].Low);
                Assert.Equal(item.Open, ohlcListDeepClone[i].Open);
                Assert.Equal(item.Volume, ohlcListDeepClone[i].Volume);
                i++;
            }
        }

        private List<Ohlc> ReadCsvFile(string path)
        {
            List<Ohlc> OhlcList = new List<Ohlc>();
            using (CsvReader csv = new CsvReader(new StreamReader(path), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    Ohlc ohlc = new Ohlc();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        switch (headers[i])
                        {
                            case "Date":
                                ohlc.Date = new DateTime(Int32.Parse(csv[i].Substring(0, 4)), Int32.Parse(csv[i].Substring(5, 2)), Int32.Parse(csv[i].Substring(8, 2)));
                                break;
                            case "Open":
                                ohlc.Open = double.Parse(csv[i], CultureInfo.InvariantCulture);
                                break;
                            case "High":
                                ohlc.High = double.Parse(csv[i], CultureInfo.InvariantCulture);
                                break;
                            case "Low":
                                ohlc.Low = double.Parse(csv[i], CultureInfo.InvariantCulture);
                                break;
                            case "Close":
                                ohlc.Close = double.Parse(csv[i], CultureInfo.InvariantCulture);
                                break;
                            case "Volume":
                                ohlc.Volume = int.Parse(csv[i]);
                                break;
                            case "Adj Close":
                                ohlc.AdjClose = double.Parse(csv[i], CultureInfo.InvariantCulture);
                                break;
                            default:
                                break;
                        }
                    }

                    OhlcList.Add(ohlc);
                }
            }

            return OhlcList;
        }

        [Fact]
        public void ADL()
        {
            ADL adl = new ADL();
            adl.Load(csvPath);
            SingleDoubleSerie serie = adl.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void OBV()
        {
            OBV obv = new OBV();
            obv.Load(csvPath);
            SingleDoubleSerie serie = obv.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void SMA()
        {
            SMA sma = new SMA(5);
            sma.Load(csvPath);
            SingleDoubleSerie serie = sma.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void EMA()
        {
            EMA ema = new EMA(10, true);
            ema.Load(csvPath);
            SingleDoubleSerie serie = ema.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void ROC()
        {
            ROC roc = new ROC(12);
            roc.Load(csvPath);
            SingleDoubleSerie serie = roc.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void RSI()
        {
            RSI rsi = new RSI(14);
            rsi.Load(csvPath);
            RSISerie serie = rsi.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.RS.Count > 0);
            Assert.True(serie.RSI.Count > 0);
        }

        [Fact]
        public void RSIWithHardCodedValues()
        {
            RSI rsi = new RSI(14);
            List<Ohlc> ohlcList = new List<Ohlc>
            {
                new Ohlc { Close = 44.34 },
                new Ohlc { Close = 44.09 },
                new Ohlc { Close = 44.15 },
                new Ohlc { Close = 43.61 },
                new Ohlc { Close = 44.33 },
                new Ohlc { Close = 44.83 },
                new Ohlc { Close = 45.10 },
                new Ohlc { Close = 45.42 },
                new Ohlc { Close = 45.84 },
                new Ohlc { Close = 46.08 },
                new Ohlc { Close = 45.89 },
                new Ohlc { Close = 46.03 },
                new Ohlc { Close = 45.61 },
                new Ohlc { Close = 46.28 },
                new Ohlc { Close = 46.28 },
                new Ohlc { Close = 46.00 },
                new Ohlc { Close = 46.03 },
                new Ohlc { Close = 46.41 },
                new Ohlc { Close = 46.22 },
                new Ohlc { Close = 45.64 }
            };
            rsi.Load(ohlcList);
            RSISerie serie = rsi.Calculate();

            Assert.NotNull(serie);
            
            Assert.True(serie.RS.Count > 0);

            Assert.True(Math.Round(serie.RS[14].Value, 2) == 2.39);
            Assert.True(Math.Round(serie.RS[15].Value, 2) == 1.96);
            Assert.True(Math.Round(serie.RS[16].Value, 2) == 1.98);
            Assert.True(Math.Round(serie.RS[17].Value, 2) == 2.26);
            Assert.True(Math.Round(serie.RS[18].Value, 2) == 1.97);
            Assert.True(Math.Round(serie.RS[19].Value, 2) == 1.38);

            Assert.True(serie.RSI.Count > 0);

            Assert.True(Math.Round(serie.RSI[14].Value, 2) == 70.46);
            Assert.True(Math.Round(serie.RSI[15].Value, 2) == 66.25);
            Assert.True(Math.Round(serie.RSI[16].Value, 2) == 66.48);
            Assert.True(Math.Round(serie.RSI[17].Value, 2) == 69.35);
            Assert.True(Math.Round(serie.RSI[18].Value, 2) == 66.29);
            Assert.True(Math.Round(serie.RSI[19].Value, 2) == 57.92);
        }

        [Fact]
        public void WMA()
        {
            WMA wma = new WMA(10);
            wma.Load(csvPath);
            SingleDoubleSerie serie = wma.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void DEMA()
        {
            DEMA dema = new DEMA(5);
            dema.Load(csvPath);
            SingleDoubleSerie serie = dema.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void MACD()
        {
            //MACD macd = new MACD();
            MACD macd = new MACD(true);
            macd.Load(csvPath);
            MACDSerie serie = macd.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Signal.Count > 0);
            Assert.True(serie.MACDLine.Count > 0);
            Assert.True(serie.MACDHistogram.Count > 0);
        }

        [Fact]
        public void Aroon()
        {
            Aroon aroon = new Aroon(5);
            aroon.Load(csvPath);
            AroonSerie serie = aroon.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Down.Count > 0);
            Assert.True(serie.Up.Count > 0);
        }

        [Fact]
        public void ATR()
        {
            ATR atr = new ATR();
            List<Ohlc> ohlcList = new List<Ohlc>
            {
                new Ohlc { High = 48.70, Low = 47.79, Close = 48.16},
                new Ohlc { High = 48.72, Low = 48.14, Close = 48.61},
                new Ohlc { High = 48.90, Low = 48.39, Close = 48.75},
                new Ohlc { High = 48.87, Low = 48.37, Close = 48.63},
                new Ohlc { High = 48.82, Low = 48.24, Close = 48.74},
                new Ohlc { High = 49.05, Low = 48.64, Close = 49.03},
                new Ohlc { High = 49.20, Low = 48.94, Close = 49.07},
                new Ohlc { High = 49.35, Low = 48.86, Close = 49.32},
                new Ohlc { High = 49.92, Low = 49.50, Close = 49.91},
                new Ohlc { High = 50.19, Low = 49.87, Close = 50.13},
                new Ohlc { High = 50.12, Low = 49.20, Close = 49.53},
                new Ohlc { High = 49.66, Low = 48.90, Close = 49.50},
                new Ohlc { High = 49.88, Low = 49.43, Close = 49.75},
                new Ohlc { High = 50.19, Low = 49.73, Close = 50.03},
                new Ohlc { High = 50.36, Low = 49.26, Close = 50.31},
                new Ohlc { High = 50.57, Low = 50.09, Close = 50.52},
                new Ohlc { High = 50.65, Low = 50.30, Close = 50.41},
                new Ohlc { High = 50.43, Low = 49.21, Close = 49.34},
                new Ohlc { High = 49.63, Low = 48.98, Close = 49.37},
                new Ohlc { High = 50.33, Low = 49.61, Close = 50.23}
            };
            atr.Load(ohlcList);

            ATRSerie serie = atr.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.ATR.Count > 0);
            Assert.True(serie.TrueRange.Count > 0);

            Assert.True(Math.Round(serie.TrueRange[13].Value, 2) == 0.46);
            Assert.True(Math.Round(serie.TrueRange[14].Value, 2) == 1.10);
            Assert.True(Math.Round(serie.TrueRange[15].Value, 2) == 0.48);
            Assert.True(Math.Round(serie.TrueRange[16].Value, 2) == 0.35);
            Assert.True(Math.Round(serie.TrueRange[17].Value, 2) == 1.22);
            Assert.True(Math.Round(serie.TrueRange[18].Value, 2) == 0.65);
            Assert.True(Math.Round(serie.TrueRange[19].Value, 2) == 0.96);

            Assert.True(Math.Round(serie.ATR[13].Value, 2) == 0.55);
            Assert.True(Math.Round(serie.ATR[14].Value, 2) == 0.59);
            Assert.True(Math.Round(serie.ATR[15].Value, 2) == 0.59);
            Assert.True(Math.Round(serie.ATR[16].Value, 2) == 0.57);
            Assert.True(Math.Round(serie.ATR[17].Value, 2) == 0.61);
            Assert.True(Math.Round(serie.ATR[18].Value, 2) == 0.62);
            Assert.True(Math.Round(serie.ATR[19].Value, 2) == 0.64);
        }

        [Fact]
        public void BollingerBand()
        {
            BollingerBand bollingerBand = new BollingerBand();
            bollingerBand.Load(csvPath);
            BollingerBandSerie serie = bollingerBand.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.BandWidth.Count > 0);
            Assert.True(serie.BPercent.Count > 0);
            Assert.True(serie.LowerBand.Count > 0);
            Assert.True(serie.MidBand.Count > 0);
            Assert.True(serie.UpperBand.Count > 0);
        }

        [Fact]
        public void CCI()
        {
            CCI cci = new CCI();
            cci.Load(csvPath);
            SingleDoubleSerie serie = cci.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void CMF()
        {
            CMF cmf = new CMF();
            cmf.Load(csvPath);
            SingleDoubleSerie serie = cmf.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void CMO()
        {
            CMO cmo = new CMO();
            cmo.Load(csvPath);
            IIndicatorSerie serie = cmo.Calculate();
            Assert.NotNull(serie);
        }

        [Fact]
        public void DPO()
        {
            DPO dpo = new DPO();
            dpo.Load(csvPath);
            SingleDoubleSerie serie = dpo.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void Envelope()
        {
            Envelope envelope = new Envelope();
            envelope.Load(csvPath);
            EnvelopeSerie serie = envelope.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Lower.Count > 0);
            Assert.True(serie.Upper.Count > 0);
        }

        [Fact]
        public void Momentum()
        {
            Momentum momentum = new Momentum();
            momentum.Load(csvPath);
            SingleDoubleSerie serie = momentum.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void Volume()
        {
            Volume volume = new Volume();
            volume.Load(csvPath);
            SingleDoubleSerie serie = volume.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void TRIX()
        {
            TRIX trix = new TRIX();
            trix.Load(csvPath);
            SingleDoubleSerie serie = trix.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void WPR()
        {
            WPR wpr = new WPR();
            wpr.Load(csvPath);
            SingleDoubleSerie serie = wpr.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void ZLEMA()
        {
            ZLEMA zlema = new ZLEMA();
            zlema.Load(csvPath);
            SingleDoubleSerie serie = zlema.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void ADX()
        {
            ADX adx = new ADX();
            adx.Load(csvPath);
            ADXSerie serie = adx.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.ADX.Count > 0);
            Assert.True(serie.DINegative.Count > 0);
            Assert.True(serie.DIPositive.Count > 0);
            Assert.True(serie.DX.Count > 0);
            Assert.True(serie.TrueRange.Count > 0);
        }

        [Fact]
        public void SAR()
        {
            SAR sar = new SAR();
            sar.Load(csvPath);
            SingleDoubleSerie serie = sar.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void PVT()
        {
            PVT pvt = new PVT();
            pvt.Load(csvPath);
            SingleDoubleSerie serie = pvt.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void VROC()
        {
            VROC vroc = new VROC(25);
            vroc.Load(csvPath);
            SingleDoubleSerie serie = vroc.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void Ichimoku()
        {
            // Not sure...
            Ichimoku ichimoku = new Ichimoku();
            ichimoku.Load(csvPath);
            IchimokuSerie serie = ichimoku.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.BaseLine.Count > 0);
            Assert.True(serie.ConversionLine.Count > 0);
            Assert.True(serie.LaggingSpan.Count > 0);
            Assert.True(serie.LeadingSpanA.Count > 0);
            Assert.True(serie.LeadingSpanB.Count > 0);
        }
    }
}
