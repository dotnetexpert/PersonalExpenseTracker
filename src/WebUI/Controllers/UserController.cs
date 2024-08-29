using Application.Commands.UserCommands;
using Application.Queries.UserQueries;
using Domain.Entities;
using Domain.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ApiController
    {
        [HttpPost("authenticate")]
        public async Task<ActionResult<ApplicationUser>> Login([FromBody] LoginViewModel loginViewModel)
        {
            return await Mediator.Send(new LoginUser() { UserName = loginViewModel.UserName, Password = loginViewModel.Password });
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> Register([FromForm] RegisterViewModel registerViewModel)
        {
            var mediatr = await Mediator.Send(new RegisterUserCommand() { RegisterViewModel = registerViewModel });
            if (mediatr != null)
                return Ok(mediatr);
            else
                return BadRequest("Email Already Exists");
        }
    }
}
