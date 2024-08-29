using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration :  IEntityTypeConfiguration<ApplicationUser>
    {


        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
    }
}

}

