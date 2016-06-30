using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public class Ichimoku: IndicatorCalculatorBase<IchimokuSerie>
    {
        protected override List<Ohlc> OhlcList { get; set; }
        public int Fast = 9;
        public int Med = 26;
        public int Slow = 26;

        public Ichimoku() 
        { 
        
        }

        public Ichimoku(int fast, int med, int slow)
        {
            this.Fast = fast;
            this.Med = med;
            this.Slow = slow;
        }

        public override IchimokuSerie Calculate()
        {
            IchimokuSerie ichimokuSerie = new IchimokuSerie();

            List<double> highList = OhlcList.Select(x => x.High).ToList();
            List<double> lowList = OhlcList.Select(x => x.Low).ToList();

            // TurningLine
            List<double?> runMaxFast = Statistics.RunMax(highList, Fast);
            List<double?> runMinFast = Statistics.RunMin(lowList, Fast);
            List<double?> runMaxMed = Statistics.RunMax(highList, Med);
            List<double?> runMinMed = Statistics.RunMin(lowList, Med);
            List<double?> runMaxSlow = Statistics.RunMax(highList, Slow);
            List<double?> runMinSlow = Statistics.RunMin(lowList, Slow);

            for (int i = 0; i < OhlcList.Count; i++)
            {   
                if (i >= Fast - 1)
                {
                    ichimokuSerie.ConversionLine.Add((runMaxFast[i] + runMinFast[i]) / 2);
                }
                else
                {
                    ichimokuSerie.ConversionLine.Add(null);
                }

                if (i >= Med - 1)
                {
                    ichimokuSerie.BaseLine.Add((runMaxMed[i] + runMinMed[i]) / 2);
                    ichimokuSerie.LeadingSpanA.Add((ichimokuSerie.BaseLine[i] + ichimokuSerie.ConversionLine[i]) / 2);
                }
                else
                {
                    ichimokuSerie.BaseLine.Add(null);
                    ichimokuSerie.LeadingSpanA.Add(null);
                }

                if (i >= Slow - 1)
                {
                    ichimokuSerie.LeadingSpanB.Add((runMaxSlow[i] + runMinSlow[i]) / 2);
                }
                else
                {
                    ichimokuSerie.LeadingSpanB.Add(null);
                }
            }

            // shift to left Med
            List<double?> laggingSpan = new List<double?>();//OhlcList.Select(x => x.Close).ToList();//new double?[OhlcList.Count];
            for (int i = 0; i < OhlcList.Count; i++)
			{   
                laggingSpan.Add(null);			    
			}
            for (int i = 0; i < OhlcList.Count; i++)
            {
                if (i >= Med - 1)
                {
                    laggingSpan[i - (Med - 1)] = OhlcList[i].Close;
                }
            }
            ichimokuSerie.LaggingSpan = laggingSpan;

            return ichimokuSerie;
        }
    }
}
