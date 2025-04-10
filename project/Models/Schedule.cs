// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Schedule.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved.
// </copyright>
// <summary>
//   Represents information about a schedule.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.Models
{
    /// <summary>
    /// Represents information about a schedule.
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Schedule"/> class.
        /// </summary>
        public Schedule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Schedule"/> class, with the ids.
        /// </summary>
        /// <param name="scheduleID">Schedule's ID.</param>
        /// <param name="doctorID">Doctor's ID.</param>
        /// <param name="shiftID">Shift's ID.</param>
        public Schedule(int scheduleID, int doctorID, int shiftID)
        {
            this.ScheduleID = scheduleID;
            this.DoctorID = doctorID;
            this.ShiftID = shiftID;
        }

        /// <summary>
        /// Gets or sets the Schedule's ID.
        /// </summary>
        public int ScheduleID { get; set; }

        /// <summary>
        /// Gets or sets the Doctor's ID.
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Gets or sets the Shift's ID.
        /// </summary>
        public int ShiftID { get; set; }
    }
}
