using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public int DoctorID { get; set; }
        public int ShiftID { get; set; }

        public Schedule() {}
        public Schedule(int scheduleID, int doctorID, int shiftID)
        {
            ScheduleID = scheduleID;
            DoctorID = doctorID;
            ShiftID = shiftID;
        }
    }
}
