using System;
using CourseApp.ModelViews;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace CourseApp.Repository
{
    /// <summary>
    /// Repository class responsible for interacting with the user wallet.
    /// It delegates calls to the IUserWalletModelView to manage the user's coin balance.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CoinsRepository"/> class.
    /// </remarks>
    /// <param name="walletModelView">The model view that interacts with the data layer for user wallet management.</param>
    public class CoinsRepository(IUserWalletModelView walletModelView) : ICoinsRepository
    {
        private readonly IUserWalletModelView walletModelView = walletModelView;

        /// <summary>
        /// Initializes a user wallet if it does not already exist, with an optional initial coin balance.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet should be initialized.</param>
        /// <param name="initialCoinBalance">The initial coin balance to set for the user. Defaults to 0.</param>
        public void InitializeUserWalletIfNotExists(int userId, int initialCoinBalance = 0)
        {
            walletModelView.InitializeUserWalletIfNotExists(userId, initialCoinBalance);
        }

        /// <summary>
        /// Gets the current coin balance of the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is to be retrieved.</param>
        /// <returns>The current coin balance of the user.</returns>
        public int GetUserCoinBalance(int userId)
        {
            return walletModelView.GetUserCoinBalance(userId);
        }

        /// <summary>
        /// Sets the coin balance of the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is to be updated.</param>
        /// <param name="updatedCoinBalance">The new coin balance to set for the user.</param>
        public void SetUserCoinBalance(int userId, int updatedCoinBalance)
        {
            walletModelView.SetUserCoinBalance(userId, updatedCoinBalance);
        }

        /// <summary>
        /// Retrieves the last login time of the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose last login time is to be retrieved.</param>
        /// <returns>The last login time of the user.</returns>
        public DateTime GetUserLastLoginTime(int userId)
        {
            return walletModelView.GetUserLastLoginTime(userId);
        }

        /// <summary>
        /// Updates the last login time of the specified user to the current date and time.
        /// </summary>
        /// <param name="userId">The ID of the user whose last login time is to be updated.</param>
        public void UpdateUserLastLoginTimeToNow(int userId)
        {
            walletModelView.UpdateUserLastLoginTimeToNow(userId);
        }

        /// <summary>
        /// Adds a specified number of coins to the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet is to be updated.</param>
        /// <param name="amountToAdd">The number of coins to add to the user's wallet.</param>
        public void AddCoinsToUserWallet(int userId, int amountToAdd)
        {
            walletModelView.AddCoinsToUserWallet(userId, amountToAdd);
        }

        /// <summary>
        /// Attempts to deduct a specified number of coins from the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user whose wallet is to be updated.</param>
        /// <param name="deductionAmount">The number of coins to deduct from the user's wallet.</param>
        /// <returns>True if the deduction was successful; otherwise, false.</returns>
        public bool TryDeductCoinsFromUserWallet(int userId, int deductionAmount)
        {
            return walletModelView.TryDeductCoinsFromUserWallet(userId, deductionAmount);
        }
    }
}
