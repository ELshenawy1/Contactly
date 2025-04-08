using Contactly.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contactly.Infrastructure.Data.Config
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.Phone).HasMaxLength(15);
            builder.Property(p => p.Address).HasMaxLength(255);
            builder.Property(p => p.Notes).HasMaxLength(500);
            builder.HasIndex(c => c.Name );
            builder.HasIndex(c => c.Phone );
            builder.HasIndex(c => c.Address );
        }

    }
}
