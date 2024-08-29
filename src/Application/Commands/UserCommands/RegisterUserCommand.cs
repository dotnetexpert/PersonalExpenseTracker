using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands
{
    public class RegisterUserCommand : IRequest<ApplicationUser>
    {
        public RegisterViewModel RegisterViewModel { get; set; }

    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApplicationUser>
    {
        private readonly IUserService _userService;
        public RegisterUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ApplicationUser> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            RegisterViewModel user = new RegisterViewModel()
            {
                Email = request.RegisterViewModel.Email,
                Password = request.RegisterViewModel.Password,
                PhoneNumber = request.RegisterViewModel.PhoneNumber,
                Name = request.RegisterViewModel.Name,
            };
            var registerUser = await _userService.Register(user);
            if (registerUser != null)
            {
                return registerUser;
            }
            else
            {
                return null;
            }
        }
    }
}
