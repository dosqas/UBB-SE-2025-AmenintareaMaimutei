using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Drug
    {
        public int DrugID { get; set; }
        public string Name { get; set; }
        public string Administration { get; set; }
        public string Specification { get; set; }
        public int Supply { get; set; }

        public Drug() { Name = "Unspecified"; Administration = "0"; Specification = "0"; }

        public Drug(int drugID, string name, string administration, string specification, int supply)
        {
            DrugID = drugID;
            Name = name;
            Administration = administration;
            Specification = specification;
            Supply = supply;
        }

    }
}
