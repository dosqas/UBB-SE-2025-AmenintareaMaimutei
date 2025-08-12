using System;
using CourseApp.Repository;
using CourseApp.ModelViews;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace CourseApp.Services
{
    /// <summary>
    /// Service responsible for managing coin-related operations for users.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CoinsService"/> class.
    /// </remarks>
    /// <param name="coinsRepository">An optional repository for accessing coins data.</param>
    public class CoinsService(ICoinsRepository? coinsRepository = null) : ICoinsService
    {
        private const int UserId = 0; // Default user ID for operations that don't specify a user

        private readonly ICoinsRepository coinsRepository = coinsRepository ?? new CoinsRepository(new UserWalletModelView());

        /// <summary>
        /// Gets the coin balance for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is being queried.</param>
        /// <returns>The coin balance of the user.</returns>
        public int GetCoinBalance(int userId)
        {
            return coinsRepository.GetUserCoinBalance(userId);
        }

        /// <summary>
        /// Tries to deduct coins from a user's wallet, based on a given cost.
        /// </summary>
        /// <param name="userId">The ID of the user trying to spend coins.</param>
        /// <param name="cost">The amount of coins to deduct.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool TrySpendingCoins(int userId, int cost)
        {
            return coinsRepository.TryDeductCoinsFromUserWallet(userId, cost);
        }

        /// <summary>
        /// Adds a specified amount of coins to the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user receiving the coins.</param>
        /// <param name="amount">The amount of coins to add.</param>
        public void AddCoins(int userId, int amount)
        {
            coinsRepository.AddCoinsToUserWallet(userId, amount);
        }

        /// <summary>
        /// Applies a daily login bonus to the user's wallet if they haven't already logged in today.
        /// </summary>
        /// <param name="userId">The ID of the user receiving the bonus. Defaults to 0.</param>
        /// <returns>True if the bonus was applied, false if the user has already logged in today.</returns>
        public bool ApplyDailyLoginBonus(int userId = 0)
        {
            DateTime lastLogin = coinsRepository.GetUserLastLoginTime(userId);
            DateTime today = DateTime.Now;
            if (lastLogin.Date < today.Date)
            {
                coinsRepository.AddCoinsToUserWallet(userId, 100);
                coinsRepository.UpdateUserLastLoginTimeToNow(userId);
                return true;
            }
            return false;
        }
    }
}