using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Model.Models;

namespace TplDemo.Model.ModelsMap
{
    public class UserRoleRelMap : IEntityTypeConfiguration<UserRoleRel>
    {
        public void Configure(EntityTypeBuilder<UserRoleRel> builder)
        {
            builder.ToTable("UserRoleRel");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();

            builder.HasOne(d => d.UserInfo).WithMany(d => d.UserRoleRel).HasForeignKey(d => d.UserId).HasConstraintName("FK_UserId_Role").OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(d => d.UserRole).WithMany(d => d.UserRoleRel).HasForeignKey(d => d.RoleId).HasConstraintName("FK_Role_UserId").OnDelete(DeleteBehavior.NoAction);
        }
    }
}
