using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DuoClassLibrary.Services;
using DuoClassLibrary.Models.Roadmap;
using System.Collections.Generic;
using WebServerTest.Models;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Models;
using DuoClassLibrary.Models.Sections;

namespace WebServerTest.Controllers
{
    public class RoadmapController : Controller
    {
        private readonly IRoadmapService _roadmapService;
        private readonly ISectionService _sectionService;
        private readonly IUserService _userService;
        private readonly IQuizService _quizService;

        public RoadmapController(IRoadmapService roadmapService, ISectionService sectionService, IUserService userService, IQuizService quizService)
        {
            _roadmapService = roadmapService;
            _sectionService = sectionService;
            _userService = userService;
            _quizService = quizService;
        }

        public async Task<IActionResult> Index()
        {
            Roadmap roadmap;
            User user;
            List<Section> sections;
            try
            {
                roadmap = await _roadmapService.GetByIdAsync(1);
                var userId = HttpContext.Session.GetInt32("UserId");
                user = await _userService.GetUserById(userId.Value);
                sections = await _sectionService.GetByRoadmapId(roadmap.Id);
            }
            catch (Exception ex)
            {
                return View(new List<SectionUnlockViewModel>());
            }
            

            int completedSections = user.NumberOfCompletedSections;

            var sectionViewModels = new List<SectionUnlockViewModel>();

            for (int i = 0; i < sections.Count; i++)
            {
                var section = sections[i];
                bool isSectionUnlocked = false;

                var quizzes = section.GetAllQuizzes().ToList();
                int completedQuizzes = 0;

                for (int j = 0; j < quizzes.Count; j++)
                {
                    var quiz = quizzes[j];
                    var IsCompleted = await _quizService.IsQuizCompleted(1, quiz.Id);
                    if(IsCompleted)
                    {
                        completedQuizzes++;
                    }
                }

                List<QuizUnlockViewModel> quizViewModels;
                bool isExamUnlocked;
                var isSectionCompleted = await _sectionService.IsSectionCompleted(1, section.Id);
                var isPreviousSectionCompleted = i > 0 && await _sectionService.IsSectionCompleted(1, sections[i - 1].Id);
                var isThisExamCompleted = await _quizService.IsExamCompleted(1, section.Exam.Id);



                if (isSectionCompleted || isThisExamCompleted)
                {
                    quizViewModels = quizzes
                        .Select(q => new QuizUnlockViewModel 
                        { 
                            Quiz = q, 
                            IsUnlocked = false, 
                            IsCompleted = true 
                        })
                        .ToList();
                    isExamUnlocked = false;
                    isSectionCompleted = true;
                }
                else if (isPreviousSectionCompleted || i == 0)
                {
                    quizViewModels = quizzes
                        .Select((q, idx) => new QuizUnlockViewModel 
                        { 
                            Quiz = q, 
                            IsUnlocked = idx == completedQuizzes, 
                            IsCompleted = idx < completedQuizzes 
                        })
                        .ToList();
                    
                    isExamUnlocked = completedQuizzes >= quizzes.Count;
                    isSectionUnlocked = true;
                }
                else
                {
                    quizViewModels = quizzes
                        .Select(q => new QuizUnlockViewModel 
                        { 
                            Quiz = q, 
                            IsUnlocked = false, 
                            IsCompleted = false 
                        })
                        .ToList();
                    isExamUnlocked = false;
                    isSectionUnlocked = false;
                }

                var exam = await _quizService.GetExamFromSection(section.Id);
                bool isExamCompleted = await _quizService.IsExamCompleted(1, exam.Id);

                sectionViewModels.Add(new SectionUnlockViewModel
                {
                    Section = section,
                    IsUnlocked = isSectionUnlocked,
                    Quizzes = quizViewModels,
                    Exam = section.Exam,
                    IsExamUnlocked = isExamUnlocked,
                    IsExamCompleted = isExamCompleted
                });
            }

            return View(sectionViewModels);
        }
    }
}