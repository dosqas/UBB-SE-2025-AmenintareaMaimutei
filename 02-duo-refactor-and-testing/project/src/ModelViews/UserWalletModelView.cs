using System;
using System.Diagnostics.CodeAnalysis;
using CourseApp.Data;
using Microsoft.Data.SqlClient;

namespace CourseApp.ModelViews
{
    [ExcludeFromCodeCoverage]
    public class UserWalletModelView : DataLink, IUserWalletModelView
    {
        private const int DefaultInitialCoinBalance = 0;

        /// <summary>
        /// Initializes the user's wallet with an initial coin balance if it does not already exist.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="initialCoinBalance">The initial coin balance to set (defaults to 0).</param>
        public void InitializeUserWalletIfNotExists(int userId, int initialCoinBalance = DefaultInitialCoinBalance)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = @"
        IF NOT EXISTS (SELECT 1 FROM UserWallet WHERE UserId = @userId)
        BEGIN
            INSERT INTO UserWallet (UserId, coinBalance, lastLogin)
            VALUES (@userId, @initialCoinBalance, GETDATE())
        END";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@initialCoinBalance", initialCoinBalance);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets the current coin balance of the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The current coin balance of the user.</returns>
        public int GetUserCoinBalance(int userId)
        {
            using var connection = GetConnection();
            connection.Open();
            string query = "SELECT coinBalance FROM UserWallet WHERE UserId = @userId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            return (int?)command.ExecuteScalar() ?? DefaultInitialCoinBalance;
        }

        /// <summary>
        /// Updates the user's coin balance to a specified value.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="updatedCoinBalance">The new coin balance to set for the user.</param>
        public void SetUserCoinBalance(int userId, int updatedCoinBalance)
        {
            using var databaseConnection = GetConnection();
            databaseConnection.Open();

            string upsertWalletQuery = @"
        IF EXISTS (SELECT 1 FROM UserWallet WHERE UserId = @userId)
            BEGIN
                UPDATE UserWallet SET coinBalance = @updatedCoinBalance WHERE UserId = @userId
            END
        ELSE
            BEGIN
                INSERT INTO UserWallet (UserId, coinBalance, lastLogin)
                VALUES (@userId, @updatedCoinBalance, GETDATE())
            END";

            using var sqlCommand = new SqlCommand(upsertWalletQuery, databaseConnection);
            sqlCommand.Parameters.AddWithValue("@userId", userId);
            sqlCommand.Parameters.AddWithValue("@updatedCoinBalance", updatedCoinBalance);
            sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets the last login time of the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The last login time of the user, or <see cref="DateTime.MinValue"/> if not available.</returns>
        public DateTime GetUserLastLoginTime(int userId)
        {
            using var databaseConnection = GetConnection();
            databaseConnection.Open();
            string selectLastLoginQuery = "SELECT lastLogin FROM UserWallet WHERE UserId = @userId";
            using var sqlCommand = new SqlCommand(selectLastLoginQuery, databaseConnection);
            sqlCommand.Parameters.AddWithValue("@userId", userId);
            return (DateTime?)sqlCommand.ExecuteScalar() ?? DateTime.MinValue;
        }

        /// <summary>
        /// Updates the user's last login time to the current date and time.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        public void UpdateUserLastLoginTimeToNow(int userId)
        {
            using var databaseConnection = GetConnection();
            databaseConnection.Open();
            string upsertLastLoginQuery = @"
        IF EXISTS (SELECT 1 FROM UserWallet WHERE UserId = @userId)
            BEGIN
                UPDATE UserWallet SET lastLogin = GETDATE() WHERE UserId = @userId
            END
        ELSE
            BEGIN
                INSERT INTO UserWallet (UserId, coinBalance, lastLogin)
                VALUES (@userId, @defaultBalance, GETDATE())
            END";

            using var sqlCommand = new SqlCommand(upsertLastLoginQuery, databaseConnection);
            sqlCommand.Parameters.AddWithValue("@userId", userId);
            sqlCommand.Parameters.AddWithValue("@defaultBalance", DefaultInitialCoinBalance);
            sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds a specified amount of coins to the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="amountToAdd">The amount of coins to add to the user's wallet.</param>
        public void AddCoinsToUserWallet(int userId, int amountToAdd)
        {
            int currentCoinBalance = GetUserCoinBalance(userId);
            SetUserCoinBalance(userId, currentCoinBalance + amountToAdd);
        }

        /// <summary>
        /// Attempts to deduct a specified amount of coins from the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="deductionAmount">The amount of coins to deduct from the user's wallet.</param>
        /// <returns>True if the deduction was successful, otherwise false.</returns>
        public bool TryDeductCoinsFromUserWallet(int userId, int deductionAmount)
        {
            int currentCoinBalance = GetUserCoinBalance(userId);

            if (currentCoinBalance >= deductionAmount)
            {
                SetUserCoinBalance(userId, currentCoinBalance - deductionAmount);
                return true;
            }

            return false;
        }
    }
}