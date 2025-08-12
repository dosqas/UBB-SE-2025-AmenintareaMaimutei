using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Duo.Api.Models
{
    public class Post
    {
        public Post()
        {
            Title = string.Empty;
            Description = string.Empty;
            Hashtags = new List<string>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        [ForeignKey("User")]
        public int UserID { get; set; }
        
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikeCount { get; set; }

        [NotMapped]
        public string? Content { get => Description; set => Description = value; }
        [NotMapped]
        public string? Username { get; set; }
        [NotMapped]
        public string? Date { get; set; }
        [NotMapped]
        public List<string> Hashtags { get; set; } = new List<string>(); 
    }
}