using System.Diagnostics.CodeAnalysis;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CoinsController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        [HttpGet("balance/{userId}")]
        public async Task<ActionResult<int>> GetUserCoinBalance(int userId)
        {
            try
            {
                var balance = await repository.GetUserCoinBalanceAsync(userId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                // Log the exception and return a meaningful error response
                Console.Error.WriteLine($"Error retrieving coin balance: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the coin balance.");
            }
        }

        [HttpPost("spend")]
        public async Task<ActionResult> SpendCoins([FromBody] SpendCoinsRequest request)
        {
            try
            {
                bool success = await repository.TryDeductCoinsFromUserWalletAsync(request.UserId, request.Cost);
                if (success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Not enough coins.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error spending coins: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the transaction.");
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCoins([FromBody] AddCoinsRequest request)
        {
            try
            {
                await repository.AddCoinsToUserWalletAsync(request.UserId, request.Amount);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception and return a meaningful error response
                Console.Error.WriteLine($"Error adding coins: {ex.Message}");
                return StatusCode(500, "An error occurred while adding coins.");
            }
        }

        [HttpPost("dailybonus")]
        public async Task<ActionResult> ApplyDailyLoginBonus([FromBody] DailyBonusRequest request)
        {
            try
            {
                DateTime lastLogin = await repository.GetUserLastLoginTimeAsync(request.UserId);

                if (lastLogin.Date < DateTime.Now.Date)
                {
                    await repository.AddCoinsToUserWalletAsync(request.UserId, 10); // Example: 10 coins bonus
                    await repository.UpdateUserLastLoginTimeToNowAsync(request.UserId);
                    return Ok();
                }
                else
                {
                    return BadRequest("Bonus already claimed today.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return a meaningful error response
                Console.Error.WriteLine($"Error applying daily login bonus: {ex.Message}");
                return StatusCode(500, "An error occurred while applying the daily login bonus.");
            }
        }

        #endregion

        #region Nested Classes

        public class SpendCoinsRequest
        {
            public int UserId { get; set; }
            public int Cost { get; set; }
        }

        public class AddCoinsRequest
        {
            public int UserId { get; set; }
            public int Amount { get; set; }
        }

        public class DailyBonusRequest
        {
            public int UserId { get; set; }
        }

        #endregion
    }
}
