using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    /// <summary>
    /// Moving Average Envelopes
    /// </summary>
    public class Envelope : IndicatorCalculatorBase<EnvelopeSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        public int Period = 20;
        public double Factor = 0.025;

        public Envelope()
        { 
        
        }

        public Envelope(int period, double factor)
        {
            this.Period = period;
            this.Factor = factor;
        }

        /// <summary>
        /// Upper Envelope: 20-day SMA + (20-day SMA x .025)
        /// Lower Envelope: 20-day SMA - (20-day SMA x .025)
        /// </summary>
        /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:moving_average_envelopes"/>
        /// <returns></returns>
        public override EnvelopeSerie Calculate()
        {
            EnvelopeSerie envelopeSerie = new EnvelopeSerie();

            SMA sma = new SMA(Period);
            sma.Load(OhlcList);
            List<double?> smaList = sma.Calculate().Values;

            for (int i = 0; i < OhlcList.Count; i++)
            {   
                if (smaList[i].HasValue)
                {
                    envelopeSerie.Lower.Add(smaList[i].Value - (smaList[i].Value * Factor));
                    envelopeSerie.Upper.Add(smaList[i].Value + (smaList[i].Value * Factor));
                }
                else
                {
                    envelopeSerie.Lower.Add(null);
                    envelopeSerie.Upper.Add(null);
                }
            }

            return envelopeSerie;
        }
    }
}
