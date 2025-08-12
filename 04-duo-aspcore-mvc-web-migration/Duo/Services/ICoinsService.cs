using System.Threading.Tasks;

namespace Duo.Services
{
    /// <summary>
    /// Interface for coin-related operations
    /// </summary>
    public interface ICoinsService
    {
        /// <summary>
        /// Gets the current coin balance for a user
        /// </summary>
        Task<int> GetCoinBalanceAsync(int userId);

        /// <summary>
        /// Attempts to spend coins from a user's wallet
        /// </summary>
        /// <returns>True if successful, false if insufficient funds</returns>
        Task<bool> TrySpendingCoinsAsync(int userId, int cost);

        /// <summary>
        /// Adds coins to a user's wallet
        /// </summary>
        Task AddCoinsAsync(int userId, int amount);

        /// <summary>
        /// Checks if user has logged in today and grants daily reward if not
        /// </summary>
        /// <returns>True if daily reward was granted, false if already logged in today</returns>
        Task<bool> ApplyDailyLoginBonusAsync(int userId = 0);
    }
}
