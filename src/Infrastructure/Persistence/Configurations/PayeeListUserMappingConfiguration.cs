﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class PayeeListUserMappingConfiguration : IEntityTypeConfiguration<PayeeListUserMapping>
    {


        public void Configure(EntityTypeBuilder<PayeeListUserMapping> builder)
        {
        }
    }

}
