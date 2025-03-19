using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    class Room
    {
        public Guid RoomID { get; set; }
        public int Capacity { get; set; }
        public Guid DepartmentID { get; set; }
        public Guid EquipmentID { get; set; }

        public Room(Guid roomID, int capacity, Guid departmentID, Guid equipmentID)
        {
            RoomID = roomID;
            Capacity = capacity;
            DepartmentID = departmentID;
            EquipmentID = equipmentID;
        }
    }
}
