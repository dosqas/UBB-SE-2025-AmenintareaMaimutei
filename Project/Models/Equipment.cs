using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Equipment
    {
        public Guid EquipmentID { get; set; }
        public string Type { get; set; }
        public string Specification { get; set; }
        public int Stock { get; set; }
        public Equipment(Guid equipmentID, string type, string specification, int stock)
        {
            EquipmentID = equipmentID;
            Type = type;
            Specification = specification;
            Stock = stock;
        }
    }
}
