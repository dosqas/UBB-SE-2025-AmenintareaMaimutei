using System.Data;
using System;
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Helpers;

public static class Mappers
{
    public static User MapUser(DataRow row)
    {
        return new User
        {
            UserId = Convert.ToInt32(row["UserId"]),
            UserName = row["UserName"].ToString()!,
            Email = row["Email"].ToString()!,
            PrivacyStatus = Convert.ToBoolean(row["PrivacyStatus"]),
            OnlineStatus = Convert.ToBoolean(row["OnlineStatus"]),
            DateJoined = Convert.ToDateTime(row["DateJoined"]),
            ProfileImage = row["ProfileImage"].ToString()!,
            TotalPoints = Convert.ToInt32(row["TotalPoints"]),
            CoursesCompleted = Convert.ToInt32(row["CoursesCompleted"]),
            QuizzesCompleted = Convert.ToInt32(row["QuizzesCompleted"]),
            Streak = Convert.ToInt32(row["Streak"]),
            Password = row["Password"].ToString()!,
            LastActivityDate = row["LastActivityDate"] == DBNull.Value ? null : (DateTime?)row["LastActivityDate"],
            Accuracy = Convert.ToDecimal(row["Accuracy"])
        };
    }
}
