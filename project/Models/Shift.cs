using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Shift
    {
        public int ShiftID { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Shift() {}

        public Shift(int shiftID, DateOnly date, TimeSpan startTime, TimeSpan endTime)
        {
            ShiftID = shiftID;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
