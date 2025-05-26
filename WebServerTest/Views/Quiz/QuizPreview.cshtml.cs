using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebServerTest.Views.Quiz
{
    public class QuizPreview : PageModel
    {
        private readonly ILogger<QuizPreview> _logger;

        public QuizPreview(ILogger<QuizPreview> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}