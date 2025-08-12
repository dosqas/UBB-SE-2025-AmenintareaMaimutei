namespace Duo.Api.DTO
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public int Cost { get; set; }
        public string ImageUrl { get; set; }
        public int TimeToComplete { get; set; }
        public string Difficulty { get; set; }
        public List<TagDto> CourseTags { get; set; }
    }
}
