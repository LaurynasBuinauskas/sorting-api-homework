namespace Sorting.Api.Homework.WebApi.Tests.Performance;

// Interface for the algorithm speed comparison
internal interface IAlgorithmSpeedComparison
{
    public IEnumerable<KeyValuePair<string, long>> GetPerformanceList(int length);
}
