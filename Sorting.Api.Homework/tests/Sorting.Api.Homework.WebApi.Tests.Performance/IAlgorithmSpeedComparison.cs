using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting.Api.Homework.WebApi.Tests.Performance;

internal interface IAlgorithmSpeedComparison
{
    public IEnumerable<KeyValuePair<string, long>> GetPerformanceList(int length);
}
