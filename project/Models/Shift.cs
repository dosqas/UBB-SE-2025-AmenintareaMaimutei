// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Shift.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved.
// </copyright>
// <summary>
// Represents information about a shift.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.Models
{
    using System;

    /// <summary>
    /// Represents information about a shift.
    /// </summary>
    public class Shift
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Shift"/> class.
        /// </summary>
        public Shift()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shift"/> class.
        /// </summary>
        /// <param name="shiftID">The shift's ID.</param>
        /// <param name="date">The date of the shift.</param>
        /// <param name="startTime">The start time of the shift.</param>
        /// <param name="endTime">The end time of the shift.</param>
        public Shift(int shiftID, DateOnly date, TimeSpan startTime, TimeSpan endTime)
        {
            this.ShiftID = shiftID;
            this.Date = date;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        /// <summary>
        /// Gets or sets the shift's ID.
        /// </summary>
        public int ShiftID { get; set; }

        /// <summary>
        /// Gets or sets the date of the shift.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the start time of the shift.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the shift.
        /// </summary>
        public TimeSpan EndTime { get; set; }
    }
}
