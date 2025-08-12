using DuoClassLibrary.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoClassLibrary.Models.Sections.DTO
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public int? SubjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RoadmapId { get; set; }
        public int? OrderNumber { get; set; }
        public List<int> QuizIds { get; set; }
        public int? ExamId { get; set; }

        public static SectionDTO ToDto(Section section)
        {
            return new SectionDTO
            {
                Id = section.Id,
                SubjectId = section.SubjectId,
                Title = section.Title,
                Description = section.Description,
                RoadmapId = section.RoadmapId,
                OrderNumber = section.OrderNumber,
                QuizIds = section.Quizzes?.ConvertAll(q => q.Id) ?? new List<int>(),
                ExamId = section.Exam?.Id
            };
        }
    }
}
