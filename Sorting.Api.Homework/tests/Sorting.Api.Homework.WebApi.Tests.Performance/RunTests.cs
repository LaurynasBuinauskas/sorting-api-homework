using Autofac;
using Sorting.Api.Homework.Tests.Performance;
using Sorting.Api.Homework.WebApi.Algorithms;

namespace Sorting.Api.Homework.WebApi.Tests.Performance;
internal class RunTests
{
    // Code to run the test
    static void Main(string[] args)
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<SortAlgorithm>().As<ISortAlgorithm>();
        builder.RegisterType<AlgorithmSpeedComparison>().As<IAlgorithmSpeedComparison>();

        using (var container = builder.Build())
        {
            var random = new Random();
            
            var sortAlgorithmSpeedComparison = container.Resolve<IAlgorithmSpeedComparison>();

            var listLength = 10000;

            var performanceResults = sortAlgorithmSpeedComparison.GetPerformanceList(listLength);

            foreach(var performanceResult in performanceResults) 
            {
                Console.WriteLine("Algorithm: {0}, execution time: {1} ms",
                    performanceResult.Key, performanceResult.Value);
            }
        }
    }

    // Results for this configuration (listLength = 10000):
    // Algorithm: MergeSort, execution time: 3 ms
    // Algorithm: InsertionSort, execution time: 145 ms
    // Algorithm: BubbleSort, execution time: 469 ms
    // From these results we can come to the conclusion that merge sort is the fastest sorting
    // algorithm from that were implemented, which is why it will be used as the default sorting algorithm
}

