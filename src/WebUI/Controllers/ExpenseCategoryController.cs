using Application.Commands.ExpenseCategoryCommands;
using Application.Queries.ExpenseCategoryQueries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : ApiController
    {
        [HttpGet("getAll")]
        public async Task<ExpenseCategoryArray> GetAll(string ApplicationUserId, int transId)
        {
            return await Mediator.Send(new GetAllExpenseCategoryByUserId() { ApplicationUserId = ApplicationUserId, TransactionId = transId });
        }

        [HttpPost("upsertCategory")]
        public async Task<ActionResult<Unit>> Upsert(ExpenseCategory category)
        {
            var mediatr = await Mediator.Send(new UpsertExpenseCategory() { ExpenseCategory = category });
            if (mediatr == 0)
                return BadRequest();
            else
                return Ok();
        }

        [HttpDelete("deleteCategory")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            var mediatr = await Mediator.Send(new DeleteExpenseCategoryCommand() { ExpenseCategoryId = id });
            if (mediatr == 0)
                return BadRequest("Error in Deleting Data");
            else
                return Ok("Data Deleted Successfully");
        }
    }
}
