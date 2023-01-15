using System.Text.Json.Serialization;

namespace Sorting.Api.Homework.WebApi.Models.DTO
{
    // Model for the algorithm choice and input array request
    public class SortRequestDTO
    {
        [JsonPropertyName(nameof(ArrayString))]
        public string ArrayString { get; set; } = string.Empty;

        [JsonPropertyName(nameof(SortAlgorithm))]
        public string SortAlgorithm { get; set; } = string.Empty;
    }
}
