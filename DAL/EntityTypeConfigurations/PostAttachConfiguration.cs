using DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntityTypeConfigurations
{
    public class PostAttachConfiguration : IEntityTypeConfiguration<PostAttach>
    {
        public void Configure(EntityTypeBuilder<PostAttach> builder)
        {
            builder.ToTable("PostAttaches");
        }
    }
}
