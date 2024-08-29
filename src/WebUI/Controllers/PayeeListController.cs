using Application.Commands.PayeeCommands;
using Application.Queries.PayeeQueries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayeeListController : ApiController
    {
        [HttpGet("getAllPayee")]
        public async Task<PayeeListArray> GetAll(string ApplicationUserId, int transId)
        {
            return await Mediator.Send(new GetAllPayeeByUserIdQuery { ApplicationUserId = ApplicationUserId, TransactionId = transId });
        }

        [HttpPost("upsertPayee")]
        public async Task<ActionResult<Unit>> Upsert([FromBody]PayeeList payee)
        {
            var result = await Mediator.Send(new UpsertPayeeCommand { Payee = payee });
            if (result == 0)
                return BadRequest();
            else
                return Ok();
        }

        [HttpDelete("deletePayee")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            var result = await Mediator.Send(new DeletePayeeCommand { PayeeId = id });
            if (result == 0)
                return BadRequest("Error in Deleting Data");
            else
                return Ok("Data Deleted Successfully");
        }
    }
}
