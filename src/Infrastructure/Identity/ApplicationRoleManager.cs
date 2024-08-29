using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Infrastructure.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(ApplicationRoleStore appRoleStore,
    IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
    ILookupNormalizer lookupNormalizer, IdentityErrorDescriber identityErrorDescriber,
    ILogger<ApplicationRoleManager> logger) : base(appRoleStore, roleValidators, lookupNormalizer,
      identityErrorDescriber, logger)
        {

        }
    }
}
