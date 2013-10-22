c-sharp-statistics-library
==========================

A Simple C# Statistics Library

I built this library originally to assist in statistical analysis of market trends in the game Eve Online. I originally posted this library on SourceForge (https://sourceforge.net/projects/cstatisticslibr/) including a DLL package. I am migrating it to GitHub in order to leverage GitHub's better update infrastructure as well as keep all of my other projects in one place.

#### Analyzer
This class contains basic Statistics functionality. It takes an array of values (int, float, double) and performs analysis on that array.

* Mean: Returns the average of the array.
* Median: Returns the median value of the array.
* Mode: Returns the most common value in the array.
* StdDevP: Returns the population standard deviation of the array.
* StdDevS: Returns the sample standard deviation of the array.
* StdDev: This function is deprecated because it calculates incorrectly.
