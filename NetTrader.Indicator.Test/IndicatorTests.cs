using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;
using Xunit.Sdk;

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
            List<Ohlc> ohlcListDeepClone = (List<Ohlc>)Utils.DeepClone(ohlcList);
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
            adl.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = adl.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void OBV()
        {
            OBV obv = new OBV();
            obv.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = obv.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void SMA()
        {
            SMA sma = new SMA(5);
            sma.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = sma.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void EMA()
        {
            EMA ema = new EMA(10, true);
            ema.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = ema.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void ROC()
        {
            ROC roc = new ROC(12);
            roc.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = roc.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void RSI()
        {
            RSI rsi = new RSI(14);
            rsi.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            RSISerie serie = rsi.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.RS.Count > 0);
            Assert.True(serie.RSI.Count > 0);
        }

        [Fact]
        public void WMA()
        {
            WMA wma = new WMA(10);
            wma.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = wma.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void DEMA()
        {
            DEMA dema = new DEMA(5);
            dema.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = dema.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void MACD()
        {
            //MACD macd = new MACD();
            MACD macd = new MACD(true);
            macd.Load(Directory.GetCurrentDirectory() + "\\table.csv");
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
            aroon.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            AroonSerie serie = aroon.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Down.Count > 0);
            Assert.True(serie.Up.Count > 0);
        }

        [Fact]
        public void ATR()
        {
            ATR atr = new ATR();
            atr.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            ATRSerie serie = atr.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.ATR.Count > 0);
            Assert.True(serie.TrueHigh.Count > 0);
            Assert.True(serie.TrueLow.Count > 0);
            Assert.True(serie.TrueRange.Count > 0);
        }

        [Fact]
        public void BollingerBand()
        {
            BollingerBand bollingerBand = new BollingerBand();
            bollingerBand.Load(Directory.GetCurrentDirectory() + "\\table.csv");
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
            cci.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = cci.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void CMF()
        {
            CMF cmf = new CMF();
            cmf.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = cmf.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void CMO()
        {
            CMO cmo = new CMO();
            cmo.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            IIndicatorSerie serie = cmo.Calculate();
            Assert.NotNull(serie);
        }

        [Fact]
        public void DPO()
        {
            DPO dpo = new DPO();
            dpo.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = dpo.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void Envelope()
        {
            Envelope envelope = new Envelope();
            envelope.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            EnvelopeSerie serie = envelope.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Lower.Count > 0);
            Assert.True(serie.Upper.Count > 0);
        }

        [Fact]
        public void Momentum()
        {
            Momentum momentum = new Momentum();
            momentum.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = momentum.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void Volume()
        {
            Volume volume = new Volume();
            volume.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = volume.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void TRIX()
        {
            TRIX trix = new TRIX();
            trix.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = trix.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void WPR()
        {
            WPR wpr = new WPR();
            wpr.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = wpr.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void ZLEMA()
        {
            ZLEMA zlema = new ZLEMA();
            zlema.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = zlema.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void ADX()
        {
            ADX adx = new ADX();
            adx.Load(Directory.GetCurrentDirectory() + "\\table.csv");
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
            sar.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = sar.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void PVT()
        {
            PVT pvt = new PVT();
            pvt.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = pvt.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void VROC()
        {
            VROC vroc = new VROC(25);
            vroc.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = vroc.Calculate();

            Assert.NotNull(serie);
            Assert.True(serie.Values.Count > 0);
        }

        [Fact]
        public void Ichimoku()
        {
            // Not sure...
            Ichimoku ichimoku = new Ichimoku();
            ichimoku.Load(Directory.GetCurrentDirectory() + "\\table.csv");
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
