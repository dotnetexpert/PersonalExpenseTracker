using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.ViewModels;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebUI;

namespace Personal_Expense_Tracker_1.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        public UserService(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager, UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings)
        {
            _applicationUserManager = applicationUserManager;
            _applicationSignInManager = applicationSignInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }
        public async Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel)
        {
            var applicationUser = await _applicationUserManager.FindByEmailAsync(loginViewModel.UserName);
            if (applicationUser != null)
            {
                var abc = await _applicationUserManager.CheckPasswordAsync(applicationUser, loginViewModel.Password);
                if (abc == true)
                {
                    applicationUser.PasswordHash = "";

                    //JWT
                    applicationUser.Role = "Individual User";

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescritor = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                    new Claim(ClaimTypes.Name,applicationUser.Id),
                    new Claim(ClaimTypes.Email,applicationUser.Email),
                    new Claim(ClaimTypes.Role,applicationUser.Role)
                    }),
                        Expires = DateTime.UtcNow.AddHours(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescritor);
                    applicationUser.Token = tokenHandler.WriteToken(token);

                    //****
                    return applicationUser;
                }
                else return null;
            }
            else
            {
                return null;
            }
        }
        public async Task<ApplicationUser> Register(RegisterViewModel registerViewModel)
        {
            var user = new ApplicationUser
            {
                Name = registerViewModel.Name,
                Email = registerViewModel.Email,
                PhoneNumber = registerViewModel.PhoneNumber,
                UserName = registerViewModel.Email.ToUpper()
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                return user;
            }
            throw new Exception("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

    }
}
