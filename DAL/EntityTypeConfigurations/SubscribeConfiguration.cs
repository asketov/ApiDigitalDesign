using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityTypeConfigurations
{
    public class SubscribeConfiguration : IEntityTypeConfiguration<Subscribe>
    {
        public void Configure(EntityTypeBuilder<Subscribe> builder)
        {
            builder.HasKey(u => new { u.SubscriberId, u.RecipientId });
        }
    }
}
