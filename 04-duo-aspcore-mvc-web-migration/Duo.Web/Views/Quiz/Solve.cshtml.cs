using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Duo.Web.Views.Quiz
{
    public class Solve : PageModel
    {
        private readonly ILogger<Solve> _logger;

        public Solve(ILogger<Solve> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}