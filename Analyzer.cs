using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace StatisticsLibrary
{
    public class Analyzer<T> where T:IComparable
    {
        private T[] m_Values;

        public Analyzer(T[] theSeed)
        {
            m_Values = theSeed;
            Array.Sort(theSeed);
        }

        /// <summary>
        /// This method returns the average of the array.
        /// </summary>
        /// <returns>Returns the average of the array.</returns>
        public double Mean()
        {
            return m_Values.Average(delegate(T s) { return Convert.ToDouble(s); });
        }

        /// <summary>
        /// This method returns the median value in the array.
        /// 
        /// If there is an even number of values, this returns the average of the 
        /// two centermost values.
        /// </summary>
        /// <returns>Returns the median of the array.</returns>
        public double Median()
        {
            int med = m_Values.Length / 2;

            return Convert.ToDouble(m_Values[med]);
        }

        /// <summary>
        /// This method returns the mode of the array.
        /// 
        /// The mode is the most common value in the array.
        /// </summary>
        /// <returns>Returns the mode of the array.</returns>
        public double Mode()
        {
            double ret = 0;

            Dictionary<T, int> values = new Dictionary<T, int>();

            foreach (T element in m_Values)
            {
                if (values.ContainsKey(element))
                    values[element]++;
                else
                    values.Add(element, 1);
            }

            int max = 0;
            foreach (KeyValuePair<T, int> value in values)
            {
                if (value.Value > max)
                {
                    ret = Convert.ToDouble(value.Key);
                    max = value.Value;
                }
            }
            return ret;
        }

        /// <summary>
        /// This method returns the Population Standard Deviation.
        /// </summary>
        /// <returns>Returns the standard deviation if there is more than one element, otherwise returns 0.</returns>
        public double StdDevP()
        {
            double ret = 0;

            if (m_Values.Length > 1)
            {
                double mean = this.Mean();
                double sum = 0;

                m_Values.ToList<T>().ForEach(v => sum += Math.Pow((Convert.ToDouble(v) - mean), 2));

                double variance = sum / m_Values.Length;
                ret = Math.Sqrt(variance);
            }

            return ret;
        }

        /// <summary>
        /// This method returns the sample standard deviation.
        /// </summary>
        /// <returns>Returns the sample standard deviation if there is more than one element, otherwise returns 0.</returns>
        public double StdDevS()
        {
            double ret = 0;

            if (m_Values.Length > 1)
            {
                double mean = this.Mean();
                double sum = 0;
                int length = m_Values.Length;

                m_Values.ToList<T>().ForEach(v => sum += Math.Pow((Convert.ToDouble(v) - mean), 2));

                double variance = sum / (length - 1);
                ret = Math.Sqrt(variance);
            }

            return ret;
        }

        /// <summary>
        /// This method has been deprecated for all future versions.
        /// 
        /// It incorrrectly calculates the standard deviation because it does not calculate the
        /// deviations correctly.
        /// </summary>
        /// <returns>Returns 0.</returns>
        public double StdDev()
        {
            double ret = 0;

            double mean = this.Mean();
            double sum = 0;

            foreach (T element in m_Values)
            {
                sum += Convert.ToDouble(element) * Convert.ToDouble(element);
            }

            double sqmean = sum / m_Values.Length;

            ret = Math.Sqrt((sqmean - (mean * mean)));

            return ret;
        }
    }

}
