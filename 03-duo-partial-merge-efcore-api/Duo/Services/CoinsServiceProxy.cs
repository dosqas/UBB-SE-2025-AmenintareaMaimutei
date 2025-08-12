using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    /// <summary>
    /// Provides methods to interact with the Coins API for managing user coin balances and transactions.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CoinsServiceProxy"/> class.
    /// </remarks>
    /// <param name="httpClient">The HTTP client used to send requests to the Coins API.</param>
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
    public class CoinsServiceProxy(HttpClient httpClient) : ICoinsServiceProxy
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly string url = "https://localhost:7174";

        /// <summary>
        /// Retrieves the coin balance for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is to be retrieved.</param>
        /// <returns>The user's coin balance, or 0 if an error occurs.</returns>
        public async Task<int> GetUserCoinBalanceAsync(int userId)
        {
            var response = await httpClient.GetFromJsonAsync<int>($"{url}/api/Coins/get-balance/{userId}");
            return response;
        }

        /// <summary>
        /// Attempts to spend a specified amount of coins for a user.
        /// </summary>
        /// <param name="userId">The ID of the user attempting to spend coins.</param>
        /// <param name="cost">The amount of coins to spend.</param>
        /// <returns><c>true</c> if the operation is successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> TrySpendingCoinsAsync(int userId, int cost)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Coins/add-spent-coins", new { UserId = userId, Cost = cost });
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Adds a specified amount of coins to a user's balance.
        /// </summary>
        /// <param name="userId">The ID of the user to whom coins will be added.</param>
        /// <param name="amount">The amount of coins to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddCoinsAsync(int userId, int amount)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Coins/add", new { UserId = userId, Amount = amount });
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Applies a daily login bonus to a user's coin balance.
        /// </summary>
        /// <param name="userId">The ID of the user receiving the daily login bonus.</param>
        /// <returns><c>true</c> if the operation is successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> ApplyDailyLoginBonusAsync(int userId)
        {
            Console.WriteLine("daily bonus endpoint hit");
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Coins/get-dailybonus", new { UserId = userId });
            return response.IsSuccessStatusCode;
        }
    }
}
