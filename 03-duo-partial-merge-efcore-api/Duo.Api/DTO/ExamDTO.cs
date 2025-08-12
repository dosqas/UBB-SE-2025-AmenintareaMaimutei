using Duo.Api.Models.Exercises;
using System.Text.Json.Serialization;

namespace Duo.Api.DTO
{
    public class ExamDTO
    {
        public int Id { get; set; }

        [JsonPropertyName("SectionId")]
        [JsonInclude] // This ensures it's included in serialization, even when null
        public int? SectionId { get; set; }

        public ICollection<Exercise> Exercises { get; set; } = [];
    }
}
