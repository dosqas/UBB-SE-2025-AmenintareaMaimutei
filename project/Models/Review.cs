namespace Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a review associated with a medical record.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Review"/> class.
        /// </summary>
        /// <param name="reviewID">The unique identifier of the review.</param>
        /// <param name="medicalRecordID">The ID of the associated medical record.</param>
        /// <param name="text">The text content of the review.</param>
        /// <param name="nrStars">The number of stars given in the review.</param>
        public Review(int reviewID, int medicalRecordID, string text, int nrStars)
        {
            this.ReviewID = reviewID;
            this.MedicalRecordID = medicalRecordID;
            this.Text = text;
            this.NrStars = nrStars;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the review.
        /// </summary>
        public int ReviewID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated medical record.
        /// </summary>
        public int MedicalRecordID { get; set; }

        /// <summary>
        /// Gets or sets the text content of the review.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the number of stars given in the review.
        /// </summary>
        public int NrStars { get; set; }
    }
}
