using System.Collections.Generic;
using DuoClassLibrary.Models;

namespace WebServerTest.Models
{
    public class CommunityViewModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public int? SelectedCategoryId { get; set; }
        public List<string> SelectedHashtags { get; set; } = new List<string>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalPosts { get; set; }
        public int ItemsPerPage { get; set; }
        public List<Hashtag> AllHashtags { get; set; } = new List<Hashtag>();
    }
} 