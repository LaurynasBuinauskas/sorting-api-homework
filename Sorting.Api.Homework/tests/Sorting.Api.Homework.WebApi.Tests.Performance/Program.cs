using Autofac;
using Sorting.Api.Homework.Tests.Performance;
using Sorting.Api.Homework.WebApi.Algorithms;

namespace Sorting.Api.Homework.WebApi.Tests.Performance;
internal class Program
{
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
}

