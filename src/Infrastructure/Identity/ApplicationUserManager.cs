using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Infrastructure.Identity
{
  public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    public ApplicationUserManager(ApplicationUserStore applicationUserStore, IOptions<IdentityOptions> options,
IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer lookupNormalizer,
IdentityErrorDescriber identityErrorDescriber, IServiceProvider service, ILogger<ApplicationUserManager> logger) : base(
 applicationUserStore, options, passwordHasher, userValidators, passwordValidators, lookupNormalizer, identityErrorDescriber, service, logger)

    {

    }
  }
}
