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
    public class PayeeListConfiguration : IEntityTypeConfiguration<PayeeList>
    {

        public void Configure(EntityTypeBuilder<PayeeList> builder)
        {
        }
    }

}