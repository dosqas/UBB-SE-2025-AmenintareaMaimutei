    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public int Capacity { get; set; }
        public int DepartmentID { get; set; }
        public int EquipmentID { get; set; }

        public Room() {}

        public Room(int roomID, int capacity, int departmentID, int equipmentID)
        {
            RoomID = roomID;
            Capacity = capacity;
            DepartmentID = departmentID;
            EquipmentID = equipmentID;
        }
    }
}
