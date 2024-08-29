using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.UserQueries
{
    public class LoginUser : IRequest<ApplicationUser>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginUserHandler : IRequestHandler<LoginUser, ApplicationUser>
    {
        private readonly IUserService _userService;
        public LoginUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ApplicationUser> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                UserName = request.UserName,
                Password = request.Password
            };
            var applicationUser = await _userService.Authenticate(loginViewModel);
            return applicationUser;
        }
    }
}
