using System.Diagnostics;
using Sorting.Api.Homework.WebApi.Algorithms;
using Sorting.Api.Homework.WebApi.Tests.Performance;
using Sorting.Api.Homework.WebApi.Constants;

namespace Sorting.Api.Homework.Tests.Performance;

public class AlgorithmSpeedComparison : IAlgorithmSpeedComparison
{
    private readonly ISortAlgorithm _sortAlgorithm;

    public AlgorithmSpeedComparison(ISortAlgorithm sortAlgorithm)
    {
        _sortAlgorithm = sortAlgorithm;
    }

    public IEnumerable<KeyValuePair<string, long>> GetPerformanceList(int length)
    {
        var algorithms = new List<string> {
            AlgorithmNames.Bubble, AlgorithmNames.Merge, AlgorithmNames.Inertion
        };

        var random = new Random();
        var numbers = Enumerable.Range(1, length).Select(x => random.Next()).ToList();

        var executionTimes = algorithms.ToDictionary(algorithm => algorithm, algorithm => MeasureExecutionTime(numbers, algorithm));

        return executionTimes.OrderBy(x => x.Value);        
    }

    private long MeasureExecutionTime(List<int> numbers, string algorithm)
    {
        var numbersCopy = new List<int>(numbers);

        var stopwatch = Stopwatch.StartNew();  
        
        _sortAlgorithm.Sort(numbersCopy, algorithm);

        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }
}
