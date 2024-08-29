using Domain.Entities;
using Domain.Entities.ViewModels;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
  public interface IUserService
  {
    Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel);
    Task<ApplicationUser>Register(RegisterViewModel registerViewModel);
  }
}
