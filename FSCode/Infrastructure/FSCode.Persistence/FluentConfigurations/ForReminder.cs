using FSCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Persistence.FluentConfigurations
{
    public class ForReminder : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.Property(x => x.SendAt)
                .IsRequired();

            builder.Property(x => x.Method)
                .IsRequired();
    
        }
    }
}
