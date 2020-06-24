using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Model.Models;

namespace TplDemo.Model.ModelsMap
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.Name).HasColumnType("varchar(36)").IsRequired();
            builder.Property(d => d.Description).HasColumnType("varchar(100)").IsRequired();
        }
    }
}
