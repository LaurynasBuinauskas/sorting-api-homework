using System.Text.Json.Serialization;

namespace Sorting.Api.Homework.WebApi.Models
{
    public class LoadedFile
    {
        public IEnumerable<int> SortedList { get; set; }

        public string FileName { get; set; } = string.Empty;
    }
}
