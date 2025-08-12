using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuoClassLibrary.Models
{
    // PENTRU POST MODEL
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }

        // Default constructor for serialization
        public Category()
        {
        }

        // Constructor for manual creation
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}