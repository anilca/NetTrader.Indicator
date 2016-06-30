using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTrader.Indicator
{
    public static class Statistics
    {
        public static double StandardDeviation(List<double> valueList)
        {
            double M = 0.0;
            double S = 0.0;
            int k = 1;
            foreach (double value in valueList)
            {
                double tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;
            }
            return Math.Sqrt(S / (k - 2));
        }

        public static List<double?> RunMax(List<double> list, int period)
        {
            List<double?> maxList = new List<double?>();

            for (int i = 0; i < list.Count; i++)
            {
                if (i >= period - 1)
                {
                    double max = 0.0;
                    for (int j = i - (period - 1); j <= i; j++)
                    {   
                        if (list[j] > max)
                        {
                            max = list[j];
                        }                                                
                    }

                    maxList.Add(max);
                }
                else
                {
                    maxList.Add(null);
                }
            }

            return maxList;
        }

        public static List<double?> RunMin(List<double> list, int period)
        {
            List<double?> minList = new List<double?>();

            for (int i = 0; i < list.Count; i++)
            {
                if (i >= period - 1)
                {
                    double min = Double.MaxValue;
                    for (int j = i - (period - 1); j <= i; j++)
                    {
                        if (list[j] < min)
                        {
                            min = list[j];
                        }
                    }

                    minList.Add(min);
                }
                else
                {
                    minList.Add(null);
                }
            }

            return minList;
        }
    }
}
