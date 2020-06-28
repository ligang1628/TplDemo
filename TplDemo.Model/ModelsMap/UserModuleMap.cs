using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Model.Models;

namespace TplDemo.Model.ModelsMap
{
    public class UserModuleMap : IEntityTypeConfiguration<UserModule>
    {
        public void Configure(EntityTypeBuilder<UserModule> builder)
        {
            builder.ToTable("UserModule");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.MId).HasColumnType("varchar(36)").IsRequired();

            builder.Property(d => d.Name).HasColumnType("varchar(36)").IsRequired();
            builder.Property(d => d.Desc).HasColumnType("varchar(200)");
            builder.Property(d => d.ParentId).HasColumnType("varchar(20)");
            builder.Property(d => d.Url).HasColumnType("varchar(50)");
            builder.Property(d => d.Icon).HasColumnType("varchar(30)");
            builder.Property(d => d.Level).HasColumnType("char(1)").IsRequired();
            builder.Property(d => d.Sequence).HasColumnType("varchar(36)");

        }
    }
}
