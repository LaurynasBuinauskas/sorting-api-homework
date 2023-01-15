namespace Sorting.Api.Homework.WebApi.Models
{
    // Model for the return object when reading the contents of a file
    public class LoadedFile
    {
        public string FileName { get; set; } = string.Empty;
        public IEnumerable<int> SortedList { get; set; }        
    }
}
