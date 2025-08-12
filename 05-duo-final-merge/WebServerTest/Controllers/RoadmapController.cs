﻿using Microsoft.AspNetCore.Mvc;
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
            int userId;
            
            try
            {
                roadmap = await _roadmapService.GetByIdAsync(1);
                var userIdFromSession = HttpContext.Session.GetInt32("UserId");
                
                if (userIdFromSession == null)
                {
                    // If no user session, redirect to login
                    return RedirectToAction("Login", "Account");
                }
                
                userId = userIdFromSession.Value;
                user = await _userService.GetUserById(userId);
                sections = await _sectionService.GetByRoadmapId(roadmap.Id);
            }
            catch (Exception ex)
            {
                // Log the error and redirect to login instead of showing empty view
                return RedirectToAction("Login", "Account");
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
                    var IsCompleted = await _quizService.IsQuizCompleted(userId, quiz.Id);
                    if(IsCompleted)
                    {
                        completedQuizzes++;
                    }
                }

                List<QuizUnlockViewModel> quizViewModels;
                bool isExamUnlocked;
                var isSectionCompleted = await _sectionService.IsSectionCompleted(userId, section.Id);
                var isPreviousSectionCompleted = i > 0 && await _sectionService.IsSectionCompleted(userId, sections[i - 1].Id);
                var isThisExamCompleted = await _quizService.IsExamCompleted(userId, section.Exam.Id);



                if (isSectionCompleted || isThisExamCompleted)
                {
                    quizViewModels = quizzes
                        .Select(q => new QuizUnlockViewModel 
                        { 
                            Quiz = q, 
                            IsUnlocked = true, 
                            IsCompleted = true 
                        })
                        .ToList();
                    isExamUnlocked = true;
                    isSectionUnlocked = true;
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
                bool isExamCompleted = await _quizService.IsExamCompleted(userId, exam.Id);

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