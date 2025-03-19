using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Review
    {
        public Guid ReviewID { get; set; }
        public Guid MedicalRecordID { get; set; }
        public string Text { get; set; }
        public int NrStars { get; set; }

        public Review(Guid reviewID, Guid medicalRecordID, string text, int nrStars)
        {
            ReviewID = reviewID;
            MedicalRecordID = medicalRecordID;
            Text = text;
            NrStars = nrStars;
        }
    }
}
