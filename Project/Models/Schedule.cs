using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Schedule
    {
        public Guid ScheduleID { get; set; }
        public Guid DoctorID { get; set; }
        public Guid ShiftID { get; set; }
        public Schedule(Guid scheduleID, Guid doctorID, Guid shiftID)
        {
            ScheduleID = scheduleID;
            DoctorID = doctorID;
            ShiftID = shiftID;
        }
    }
}
