using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Model.Models;

namespace TplDemo.Model.ModelsMap
{
    public class UserInfoMap : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("UserInfo");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();

            builder.Property(d => d.Name).HasColumnType("varchar(36)").IsRequired();
            builder.Property(d => d.Password).HasColumnType("varchar(36)").IsRequired();
            builder.Property(d => d.Email).HasColumnType("varchar(36)").IsRequired();
            builder.Property(d => d.Sex).HasColumnType("bit");
            builder.Property(d => d.Age).HasColumnType("int").IsRequired();
            builder.Property(d => d.Birth).HasColumnType("datetime");
            builder.Property(d => d.Address).HasColumnType("varchar(100)");
            builder.Property(d => d.CreateTime).HasColumnType("datetime");
            builder.Property(d => d.IP).HasColumnType("varchar(20)");
        }
    }
}
