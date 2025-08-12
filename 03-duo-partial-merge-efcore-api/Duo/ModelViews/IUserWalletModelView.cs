using System;

namespace Duo.ModelViews
{
    /// <summary>
    /// Defines the methods for interacting with a user's wallet, including adding and deducting coins,
    /// retrieving balances, and managing last login times.
    /// </summary>
    public interface IUserWalletModelView
    {
        /// <summary>
        /// Adds a specified amount of coins to a user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet will be updated.</param>
        /// <param name="amountToAdd">The number of coins to add to the user's wallet.</param>
        void AddCoinsToUserWallet(int userId, int amountToAdd);

        /// <summary>
        /// Retrieves the current coin balance of a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose balance is to be retrieved.</param>
        /// <returns>The current coin balance of the user.</returns>
        int GetUserCoinBalance(int userId);

        /// <summary>
        /// Retrieves the last login time of a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose last login time is to be retrieved.</param>
        /// <returns>The last login time of the user.</returns>
        DateTime GetUserLastLoginTime(int userId);

        /// <summary>
        /// Initializes the user's wallet if it doesn't already exist, with an optional initial coin balance.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet is to be initialized.</param>
        /// <param name="initialCoinBalance">The initial balance of coins for the user (default is 0).</param>
        void InitializeUserWalletIfNotExists(int userId, int initialCoinBalance = 0);

        /// <summary>
        /// Sets the coin balance of a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is to be updated.</param>
        /// <param name="updatedCoinBalance">The updated coin balance for the user.</param>
        void SetUserCoinBalance(int userId, int updatedCoinBalance);

        /// <summary>
        /// Attempts to deduct a specified amount of coins from the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet will be deducted.</param>
        /// <param name="deductionAmount">The amount of coins to deduct from the user's wallet.</param>
        /// <returns><c>true</c> if the deduction is successful; otherwise, <c>false</c>.</returns>
        bool TryDeductCoinsFromUserWallet(int userId, int deductionAmount);

        /// <summary>
        /// Updates the last login time of the user to the current time.
        /// </summary>
        /// <param name="userId">The ID of the user whose last login time will be updated.</param>
        void UpdateUserLastLoginTimeToNow(int userId);
    }
}