using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NetTrader.Indicator;

namespace NetTrader.Indicator.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ADL()
        {
            // OK!
            ADL adl = new ADL();
            adl.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = adl.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void OBV()
        {
            OBV obv = new OBV();
            obv.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = obv.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void SMA()
        {
            SMA sma = new SMA(5);
            sma.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = sma.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void EMA()
        {
            EMA ema = new EMA(10, true);
            ema.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = ema.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void ROC()
        {
            ROC roc = new ROC(12);
            roc.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = roc.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void RSI()
        {
            RSI rsi = new RSI(14);
            rsi.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            RSISerie serie = rsi.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.RS.Count > 0);
            Assert.IsTrue(serie.RSI.Count > 0);
        }

        [TestMethod]
        public void WMA()
        {
            WMA wma = new WMA(10);
            wma.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = wma.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void DEMA()
        {
            DEMA dema = new DEMA(5);
            dema.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = dema.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void MACD()
        {
            //MACD macd = new MACD();
            MACD macd = new MACD(true); 
            macd.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            MACDSerie serie = macd.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Signal.Count > 0);
            Assert.IsTrue(serie.MACDLine.Count > 0);
            Assert.IsTrue(serie.MACDHistogram.Count > 0);
        }

        [TestMethod]
        public void Aroon()
        {
            Aroon aroon = new Aroon(5);
            aroon.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            AroonSerie serie = aroon.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Down.Count > 0);
            Assert.IsTrue(serie.Up.Count > 0);
        }

        [TestMethod]
        public void ATR()
        {
            ATR atr = new ATR();
            atr.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            ATRSerie serie = atr.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.ATR.Count > 0);
            Assert.IsTrue(serie.TrueHigh.Count > 0);
            Assert.IsTrue(serie.TrueLow.Count > 0);
            Assert.IsTrue(serie.TrueRange.Count > 0);
        }

        [TestMethod]
        public void BollingerBand()
        {
            BollingerBand bollingerBand = new BollingerBand();
            bollingerBand.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            BollingerBandSerie serie = bollingerBand.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.BandWidth.Count > 0);
            Assert.IsTrue(serie.BPercent.Count > 0);
            Assert.IsTrue(serie.LowerBand.Count > 0);
            Assert.IsTrue(serie.MidBand.Count > 0);
            Assert.IsTrue(serie.UpperBand.Count > 0);
        }

        [TestMethod]
        public void CCI()
        {
            CCI cci = new CCI();
            cci.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = cci.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void CMF()
        {
            CMF cmf = new CMF();
            cmf.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = cmf.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void CMO()
        {
            CMO cmo = new CMO();
            cmo.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            IIndicatorSerie serie = cmo.Calculate();
            Assert.IsNotNull(serie);
        }

        [TestMethod]
        public void DPO()
        {
            DPO dpo = new DPO();
            dpo.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = dpo.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void Envelope()
        {
            Envelope envelope = new Envelope();
            envelope.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            EnvelopeSerie serie = envelope.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Lower.Count > 0);
            Assert.IsTrue(serie.Upper.Count > 0);
        }

        [TestMethod]
        public void Momentum()
        {
            Momentum momentum = new Momentum();
            momentum.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = momentum.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void Volume()
        {
            Volume volume = new Volume();
            volume.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = volume.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void TRIX()
        {
            TRIX trix = new TRIX();
            trix.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = trix.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void WPR()
        {
            WPR wpr = new WPR();
            wpr.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = wpr.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void ZLEMA()
        {
            ZLEMA zlema = new ZLEMA();
            zlema.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = zlema.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void ADX()
        {
            ADX adx = new ADX();
            adx.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            ADXSerie serie = adx.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.ADX.Count > 0);
            Assert.IsTrue(serie.DINegative.Count > 0);
            Assert.IsTrue(serie.DIPositive.Count > 0);
            Assert.IsTrue(serie.DX.Count > 0);
            Assert.IsTrue(serie.TrueRange.Count > 0);
        }

        [TestMethod]
        public void SAR()
        {
            SAR sar = new SAR();
            sar.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = sar.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void PVT()
        {
            PVT pvt = new PVT();
            pvt.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = pvt.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void VROC()
        {
            VROC vroc = new VROC(25);
            vroc.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            SingleDoubleSerie serie = vroc.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.Values.Count > 0);
        }

        [TestMethod]
        public void Ichimoku()
        {
            // Not sure...
            Ichimoku ichimoku = new Ichimoku();
            ichimoku.Load(Directory.GetCurrentDirectory() + "\\table.csv");
            IchimokuSerie serie = ichimoku.Calculate();

            Assert.IsNotNull(serie);
            Assert.IsTrue(serie.BaseLine.Count > 0);
            Assert.IsTrue(serie.ConversionLine.Count > 0);
            Assert.IsTrue(serie.LaggingSpan.Count > 0);
            Assert.IsTrue(serie.LeadingSpanA.Count > 0);
            Assert.IsTrue(serie.LeadingSpanB.Count > 0);
        }
        
    }
}
