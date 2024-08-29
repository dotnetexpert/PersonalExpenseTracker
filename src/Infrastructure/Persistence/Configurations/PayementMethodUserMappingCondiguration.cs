using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class PayementMethodUserMappingCondiguration : IEntityTypeConfiguration<PayementMethodUserMapping>
    {


        public void Configure(EntityTypeBuilder<PayementMethodUserMapping> builder)
        {
        }
    }

}
