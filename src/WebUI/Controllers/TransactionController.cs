using Application.Commands.TransactionCommands;
using Application.Queries.TransactionQueries;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ApiController
    {
        [HttpGet("getAllTransactions")]
        public async Task<GetAllTransactionsByTransactionIdVM> GetAll(string userId)
        {
            return await Mediator.Send(new GetAllTransactionsByTransactionId() { UserId = userId });
        }

        [HttpPost("upsertTransaction")]
        public async Task<ActionResult> Upsert([FromBody] Transaction transaction)
        {
            var mediatr = await Mediator.Send(new UpsertTransactionCommand() { Transaction = transaction });
            if (mediatr == 0) return BadRequest();
            return Ok();
        }

        [HttpDelete("deleteTransaction")]
        public async Task<int> Delete(int id)
        {
            return await Mediator.Send(new DeleteTransactionByIdCommand() { TransactionId = id });
        }

        //        /// <summary>
        //        /// Retrieves today's transactions for a specific user.
        //        /// </summary>
        //        /// <param name="userId">The ID of the user whose transactions are to be retrieved.</param>
        //        /// <returns>A summary of today's transactions.</returns>    
        [HttpGet("getTodayTransactions")]
        public async Task<TransactionSummaryVM> GetTodayTransactions(string userId,DateTime startDate,DateTime endDate)
        {
            return await Mediator.Send(new GetTransactionsForPeriod() { UserId = userId, StartDate = startDate, EndDate = endDate });
        }
        //        /// <summary>
        //        /// Retrieves transactions from the last 7 days for a specific user.
        //        /// </summary>
        //        /// <param name="userId">The ID of the user whose transactions are to be retrieved.</param>
        //        /// <returns>A summary of the last 7 days' transactions.</returns>
        [HttpGet("getLast7DaysTransactions")]
        public async Task<TransactionSummaryVM> GetLast7DaysTransactions(string userId)
        {
            var today = DateTime.Today;
            var last7Days = today.AddDays(-7);
            return await Mediator.Send(new GetTransactionsForPeriod() { UserId = userId, StartDate = today, EndDate = last7Days });

        }
        //        /// <summary>
        //        /// Retrieves this month's transactions for a specific user.
        //        /// </summary>
        //        /// <param name="userId">The ID of the user whose transactions are to be retrieved.</param>
        //        /// <returns>A summary of this month's transactions.</returns>    
        [HttpGet("getThisMonthTransactions")]
        public async Task<TransactionSummaryVM> GetThisMonthTransactions(string userId)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            return await Mediator.Send(new GetTransactionsForPeriod() { UserId = userId, StartDate = today, EndDate = startOfMonth });
        }
        //        /// <summary>
        //        /// Retrieves this year's transactions for a specific user.
        //        /// </summary>
        //        /// <param name="userId">The ID of the user whose transactions are to be retrieved.</param>
        //        /// <returns>A summary of this year's transactions.</returns>  
        [HttpGet("getThisYearTransactions")]
        public async Task<TransactionSummaryVM> GetThisYearTransactions(string userId)
        {
            var today = DateTime.Today;
            var startOfYear = new DateTime(today.Year, 1, 1);
            return await Mediator.Send(new GetTransactionsForPeriod() { UserId = userId, StartDate = today, EndDate = startOfYear });
        }
    }
}