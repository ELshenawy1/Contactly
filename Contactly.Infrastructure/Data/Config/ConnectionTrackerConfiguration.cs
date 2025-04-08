using Contactly.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contactly.Infrastructure.Data.Config
{
    public class ConnectionTrackerConfiguration : IEntityTypeConfiguration<ConnectionTracker>
    {
        public void Configure(EntityTypeBuilder<ConnectionTracker> builder)
        {
            builder.HasKey(c => c.ConnectionId);
            builder.Property(c=> c.ContactId).HasMaxLength(255);
        }
    }
}
