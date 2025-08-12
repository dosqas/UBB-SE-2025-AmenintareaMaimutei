using DuoClassLibrary.Models.Sections;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuoClassLibrary.Models
{
    public class SectionCompletions
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who completed the section.
        /// </summary>
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the completed section.
        /// </summary>
        [ForeignKey(nameof(Section))]
        public int SectionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the section has been completed.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Navigation property to the related user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Navigation property to the related section.
        /// </summary>
        public Section Section { get; set; }

        public SectionCompletions()
        {
        }
    }
}
