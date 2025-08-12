using System;
using System.Diagnostics.CodeAnalysis;
using CourseApp.Data;
using Microsoft.Data.SqlClient;

namespace CourseApp.ModelViews
{
    [ExcludeFromCodeCoverage]
    internal class RewardModelView : DataLink
    {
        /// <summary>
        /// Claims the completion reward for a user if it hasn't been claimed yet.
        /// </summary>
        /// <param name="userId">The ID of the user claiming the reward.</param>
        /// <param name="courseId">The ID of the course being completed.</param>
        /// <returns>Returns true if the reward is claimed successfully; otherwise, false.</returns>
        public static bool ClaimCompletionReward(int userId, int courseId)
        {
            bool claimed = false;
            using (var connection = DataLink.GetConnection())
            {
                connection.Open();

                // First check if it's already claimed
                string checkQuery = @"
                    SELECT CompletionRewardClaimed 
                    FROM CourseCompletions 
                    WHERE UserId = @userId AND CourseId = @courseId";

                bool alreadyClaimed = false;
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@userId", userId);
                    checkCommand.Parameters.AddWithValue("@courseId", courseId);
                    var result = checkCommand.ExecuteScalar();
                    alreadyClaimed = result != null && (bool)result;
                }

                if (!alreadyClaimed)
                {
                    // If not claimed, update the reward status
                    string updateQuery = @"
            UPDATE CourseCompletions
            SET CompletionRewardClaimed = 1
            WHERE UserId = @userId AND CourseId = @courseId";

                    using var updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@userId", userId);
                    updateCommand.Parameters.AddWithValue("@courseId", courseId);
                    updateCommand.ExecuteNonQuery();
                    claimed = true;
                }
            }
            return claimed;
        }

        /// <summary>
        /// Claims the timed reward if the user completes the course within the time limit.
        /// </summary>
        /// <param name="userId">The ID of the user claiming the reward.</param>
        /// <param name="courseId">The ID of the course being completed.</param>
        /// <param name="timeSpent">The time spent by the user to complete the course.</param>
        /// <param name="timeLimit">The time limit for course completion.</param>
        /// <returns>Returns true if the reward is claimed successfully; otherwise, false.</returns>
        public static bool ClaimTimedReward(int userId, int courseId, int timeSpent, int timeLimit)
        {
            bool claimed = false;

            // Only claim if completed within time limit
            if (timeSpent <= timeLimit)
            {
                using var connection = DataLink.GetConnection();
                connection.Open();

                // Check if already claimed
                string checkQuery = @"
            SELECT TimedRewardClaimed 
            FROM CourseCompletions 
            WHERE UserId = @userId AND CourseId = @courseId";

                bool alreadyClaimed = false;
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@userId", userId);
                    checkCommand.Parameters.AddWithValue("@courseId", courseId);
                    var result = checkCommand.ExecuteScalar();
                    alreadyClaimed = result != null && (bool)result;
                }

                if (!alreadyClaimed)
                {
                    // If not claimed, update the reward status
                    string updateQuery = @"
                UPDATE CourseCompletions
                SET TimedRewardClaimed = 1
                WHERE UserId = @userId AND CourseId = @courseId";

                    using var updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@userId", userId);
                    updateCommand.Parameters.AddWithValue("@courseId", courseId);
                    updateCommand.ExecuteNonQuery();
                    claimed = true;
                }
            }
            return claimed;
        }

        /// <summary>
        /// Retrieves the time limit for completing a given course.
        /// </summary>
        /// <param name="courseId">The ID of the course to retrieve the time limit for.</param>
        /// <returns>Returns the time limit for the course in minutes.</returns>
        public static int GetCourseTimeLimit(int courseId)
        {
            int timeLimit = 0;
            using (var connection = DataLink.GetConnection())
            {
                connection.Open();
                string query = "SELECT TimeToComplete FROM Courses WHERE CourseId = @courseId";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@courseId", courseId);
                var result = command.ExecuteScalar();

                // Check for null or DBNull before conversion
                if (result != null && result != DBNull.Value)
                {
                    timeLimit = Convert.ToInt32(result);
                }
            }
            return timeLimit;
        }
    }
}
