using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticsLibrary
{
    public class Analyzer<T> where T : IComparable
    {
        private T[] m_Values;

        public Analyzer(T[] theSeed)
        {
            if (theSeed.Count() == 0)
                throw new Exception("You must provide a valid dataset with at least 1 item.");

            m_Values = theSeed;
            Array.Sort(theSeed);
            Count = m_Values.Count();
        }

        #region Basic Functions
        /// <summary>
        /// This method returns the average of the array.
        /// </summary>
        /// <returns>Returns the average of the array.</returns>
        public double Mean()
        {
            return m_Values.Average(x => Convert.ToDouble(x));
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
            if (m_Values.Length % 2 != 0)
            {
                return (Convert.ToDouble(m_Values[med]) + Convert.ToDouble(m_Values[med + 1])) / 2;
            }

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
            return m_Values.GroupBy(i => i)
                .OrderBy(i => i.Count())
                .Select(g => g.Key)
                .First();
        }

        /// <summary>
        /// Calculates the Standard Z Score of a given value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double StandardizedScore(T value)
        {
            return (Convert.ToDouble(value) - Mean()) / PopulationStandardDeviation();
        }
        #endregion

        #region Range Functions
        /// <summary>
        /// Returns the largest value in the dataset
        /// </summary>
        /// <returns></returns>
        public T Max()
        {
            return m_Values.Max();
        }

        /// <summary>
        /// Returns the smallest value in the dataset
        /// </summary>
        /// <returns></returns>
        public T Min()
        {
            return m_Values.Min();
        }

        /// <summary>
        /// Returns the effective range of the dataset
        /// </summary>
        /// <returns></returns>
        public T Range()
        {
            dynamic a = m_Values.Max();
            dynamic b = m_Values.Min();

            return a - b;
        }

        /// <summary>
        /// Returns the first quartile, or median of the lower half of the dataset
        /// </summary>
        /// <returns></returns>
        public T FirstQuartile()
        {
            var median = Median();
            var filtered = m_Values.Where(x => Convert.ToDouble(x) <= median);

            return MedianWith(filtered);
        }

        /// <summary>
        /// Returns the third quartile, or median of the upper half of the dataset
        /// </summary>
        /// <returns></returns>
        public T ThirdQuartile()
        {
            var median = Median();
            var filtered = m_Values.Where(x => Convert.ToDouble(x) >= median);

            return MedianWith(filtered);
        }

        /// <summary>
        /// Returns the interquartile range, the distance between the first and third quartiles
        /// </summary>
        /// <returns></returns>
        public T InterquartileRange()
        {
            dynamic a = FirstQuartile();
            dynamic b = ThirdQuartile();
            return b - a;
        }
        #endregion

        #region Population Functions
        /// <summary>
        /// Calculates the variance of the population
        /// </summary>
        /// <returns></returns>
        public double PopulationVariance()
        {
            var mean = Mean();
            var sum = SumOf(x => x - mean, x => Math.Pow(x, 2));

            return sum / Count;
        }

        /// <summary>
        /// Calculates the standard deviation, sigma, of the population
        /// </summary>
        /// <returns></returns>
        public double PopulationStandardDeviation()
        {
            return Math.Sqrt(PopulationVariance());
        }

        /// <summary>
        /// Calculates the skewness of the population.
        /// Skewness is a relationship between the standard deviation and the third moment of the mean
        /// </summary>
        /// <returns></returns>
        public double PopulationSkewness()
        {
            var mean = Mean();
            var sum = SumOf(x => x - mean, x => Math.Pow(x, 3));

            return (1d / Count) * (sum / PopulationStandardDeviation());
        }

        /// <summary>
        /// Calculates the kurtosis of the population.
        /// Kurtosis is the relationship between the second and fourth moments of the mean
        /// </summary>
        /// <returns></returns>
        public double PopulationKurtosis()
        {
            var mean = Mean();

            var fourth = SumOf(x => x - mean, x => Math.Pow(x, 4));
            var second = SumOf(x => x - mean, x => Math.Pow(x, 2));

            if (second == 0)
                throw new Exception("Cannot calculate the population kurtosis with this dataset.");

            return Count * (fourth / Math.Pow(second, 2));
        }
        #endregion

        #region Sample Functions
        /// <summary>
        /// Calculates the variance of the sample
        /// </summary>
        /// <returns></returns>
        public double SampleVariance()
        {
            if (Count <= 1)
                throw new Exception("You must provide more than one value.");

            var mean = Mean();
            var sum = SumOf(x => x - mean, x => Math.Pow(x, 2));

            return sum / (Count - 1);
        }

        /// <summary>
        /// Calculates the standard deviation of the sample
        /// </summary>
        /// <returns></returns>
        public double SampleStandardDeviation()
        {
            return Math.Sqrt(SampleVariance());
        }

        /// <summary>
        /// Calculates the skewness of the sample. See PopulationSkewness for more information
        /// </summary>
        /// <returns></returns>
        public double SampleSkewness()
        {
            if (Count < 3)
                throw new Exception("In order to calculate sample skewness, you must provide at least three values.");

            var mean = Mean();
            var sum = SumOf(x => x - mean, x => Math.Pow(x, 3));

            
            return (Count / ((Count - 1d) * (Count - 2))) * (sum / SampleStandardDeviation());
        }

        /// <summary>
        /// Calculates the kurtosis of the sample. See PopulationKurtosis for more information
        /// </summary>
        /// <returns></returns>
        public double SampleKurtosis()
        {
            if (Count < 4)
                throw new Exception("You must provide at least four values in order to calculate the sample kurtosis.");

            var interim = PopulationKurtosis() / Count;

            return ((Count * (Count + 1) * (Count - 1)) / ((Count - 2) * (Count - 3))) * interim;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Helper method that calculates the sum of values based on passed in callback functions.
        /// For instance, passing in the difference from the mean and then raising the value to a power
        /// </summary>
        /// <param name="fns"></param>
        /// <returns></returns>
        private double SumOf(params Func<double, double>[] fns)
        {
            var values = m_Values.Select(x => Convert.ToDouble(x));
            foreach (var fn in fns)
            {
                values = values.Select(fn);
            }

            return values.Aggregate(0d, (a, x) => a + x);
        }
    
        /// <summary>
        /// This method helps calculate the median value for a subset of the main dataset
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private T MedianWith(IEnumerable<T> items)
        {
            int med = items.Count() / 2;
            if (items.Count() % 2 != 0)
            {
                dynamic a = items.ElementAt(med);
                dynamic b = items.ElementAt(med + 1);
                return (a + b) / 2;
            }

            return items.ElementAt(med);
        }
        #endregion

        public int Count { get; set; }
    }
}
