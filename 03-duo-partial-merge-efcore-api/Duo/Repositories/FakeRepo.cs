using System;
using System.Diagnostics.CodeAnalysis;
using Duo.Repositories;

namespace Duo.Repository
{
    [ExcludeFromCodeCoverage]
    public class FakeCoinsRepository : ICoinsRepository
    {
        private int userCoinBalance = 100;
        private DateTime lastLogin = DateTime.Now.AddDays(-2);

        /// <summary>
        /// Gets the coin balance for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is being retrieved.</param>
        /// <returns>The current coin balance of the user.</returns>
        public int GetUserCoinBalance(int userId)
        {
            return userCoinBalance;
        }

        /// <summary>
        /// Attempts to deduct coins from the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet is being modified.</param>
        /// <param name="cost">The amount of coins to deduct.</param>
        /// <returns>True if the coins were successfully deducted; otherwise, false.</returns>
        public bool TryDeductCoinsFromUserWallet(int userId, int cost)
        {
            if (userCoinBalance >= cost)
            {
                userCoinBalance -= cost;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds coins to the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet is being updated.</param>
        /// <param name="amount">The amount of coins to add to the wallet.</param>
        public void AddCoinsToUserWallet(int userId, int amount)
        {
            userCoinBalance += amount;
        }

        /// <summary>
        /// Gets the last login time of the user.
        /// </summary>
        /// <param name="userId">The ID of the user whose last login time is being retrieved.</param>
        /// <returns>The last login time of the user.</returns>
        public DateTime GetUserLastLoginTime(int userId)
        {
            return lastLogin;
        }

        /// <summary>
        /// Updates the last login time for the user to the current time.
        /// </summary>
        /// <param name="userId">The ID of the user whose last login time is being updated.</param>
        public void UpdateUserLastLoginTimeToNow(int userId)
        {
            lastLogin = DateTime.Now;
        }

        /// <summary>
        /// Initializes the user's wallet if it does not already exist, setting an initial coin balance.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet is being initialized.</param>
        /// <param name="initialCoinBalance">The initial coin balance to set for the user.</param>
        public void InitializeUserWalletIfNotExists(int userId, int initialCoinBalance = 0)
        {
            userCoinBalance += initialCoinBalance;
        }

        /// <summary>
        /// Sets the user's coin balance to a specific value.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is being set.</param>
        /// <param name="updatedCoinBalance">The new coin balance to set for the user.</param>
        public void SetUserCoinBalance(int userId, int updatedCoinBalance)
        {
            userCoinBalance = updatedCoinBalance;
        }
    }
}
