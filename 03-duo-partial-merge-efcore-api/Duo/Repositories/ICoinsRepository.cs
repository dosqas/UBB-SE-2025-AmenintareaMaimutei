using System;

namespace Duo.Repositories
{
    /// <summary>
    /// Interface for coin-related data operations
    /// </summary>
    public interface ICoinsRepository
    {
        /// <summary>
        /// Initializes a user's wallet if it doesn't exist
        /// </summary>
        void InitializeUserWalletIfNotExists(int userId, int initialCoinBalance = 0);

        /// <summary>
        /// Gets the current coin balance for a user
        /// </summary>
        int GetUserCoinBalance(int userId);

        /// <summary>
        /// Sets the coin balance for a user (creates wallet if needed)
        /// </summary>
        void SetUserCoinBalance(int userId, int updatedCoinBalance);

        /// <summary>
        /// Gets the last login time for a user
        /// </summary>
        DateTime GetUserLastLoginTime(int userId);

        /// <summary>
        /// Updates the last login time to current time
        /// </summary>
        void UpdateUserLastLoginTimeToNow(int userId);

        /// <summary>
        /// Adds coins to a user's wallet
        /// </summary>
        void AddCoinsToUserWallet(int userId, int amountToAdd);

        /// <summary>
        /// Attempts to deduct coins from a user's wallet
        /// </summary>
        /// <returns>True if deduction was successful, false if insufficient funds</returns>
        bool TryDeductCoinsFromUserWallet(int userId, int deductionAmount);
    }
}
