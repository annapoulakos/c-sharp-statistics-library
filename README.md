c-sharp-statistics-library
==========================

A Simple C# Statistics Library

I built this library originally to assist in statistical analysis of market trends in the game Eve Online. I originally posted this library on SourceForge (https://sourceforge.net/projects/cstatisticslibr/) including a DLL package. I am migrating it to GitHub in order to leverage GitHub's better update infrastructure as well as keep all of my other projects in one place.

#### Analyzer
This class contains basic Statistics functionality. It takes an array of values (int, float, double) and performs analysis on that array.

*Mean*
> Returns the average of all values in the dataset.

*Median*
> Returns the median value in the dataset. For datasets with a shared median value, this method returns the average of the two.

*Mode*
> Returns the most common value in the dataset. If there is a tie, the smallest value will be returned.

*StandardizedScore*
> Returns the Z Score of a given value, which is the difference of the mean divided by the standard deviation.

*Min*
> Returns the smallest value in the dataset.

*Max*
> Returns the largest value in the dataset.

*Range*
> Returns the difference between the Max and Min values.

*FirstQuartile*
> Returns the first quartile, or median of the lower half of the dataset.

*ThirdQuartile*
> Returns the third quartile, or median of the upper half of the dataset.

*InterquartileRange*
> Returns the different between the third and first quartile values.

*PopulationVariance*
> Returns the variance of the population dataset.

*PopulationStandardDeviation*
> Returns the standard deviation of the population dataset.

*PopulationSkewness*
> Returns the skewness of the population dataset. See below for more information about skewness and kurtosis.

*PopulationKurtosis*
> Returns the kurtosis of the population dataset.

*SampleVariance*
> Returns the variance of the sample dataset.

*SampleStandardDeviation*
> Returns the standard deviation of the sample dataset.

*SampleSkewness*
> Returns the skewness of the sample dataset. See below for more information about skewness and kurtosis.

*SampleKurtosis*
> Returns the kurtosis of the sample dataset.

###Helper Methods
*SumOf(params Funct<double, double>[] fns)*
> Returns the aggregated sum of values after being passed to the provided function callbacks. Useful for generating a sum of the difference of squares for the variance formulas.

*MedianWith(IEnumerable<T> items)*
> Returns the median of a subset of items.

###Additional information

**Skewness**
Skewness is the ratio of the third moment of the mean (sum of difference of cubes) and the standard deviation.

***Kurtosis**
Kurtosis is the ratio of the fourth moment of the mean (sum of difference of fourths) and the second moment of the mean (sum of difference of squares).
