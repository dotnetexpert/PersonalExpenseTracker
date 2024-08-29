using Application.Commands.PaymentMethodCommands;
using Application.Queries.PaymentMethodQueries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ApiController
    {
        [HttpGet("getAllMethods")]
        public async Task<PaymentMethodArray> GetAll(string ApplicationUserId)
        {
            return await Mediator.Send(new GetAllPaymentMethodsQuery { ApplicationUserId = ApplicationUserId });
        }

        [HttpPost("upsertMethod")]
        public async Task<ActionResult<Unit>> Upsert(PaymentMethod paymentMethod)
        {
            var result = await Mediator.Send(new UpsertPaymentMethodCommand { PaymentMethod = paymentMethod });
            if (result == 0)
                return BadRequest("Error in upserting the payment method.");
            else
                return Ok("Payment method upserted successfully.");
        }

        [HttpDelete("deleteMethod")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            var result = await Mediator.Send(new DeletePaymentMethodCommand { PaymentMethodId = id });
            if (result == 0)
                return BadRequest("Error in deleting the payment method.");
            else
                return Ok("Payment method deleted successfully.");
        }
    }
}
