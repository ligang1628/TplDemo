using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Model.Models;

namespace TplDemo.Model.ModelsMap
{
    public class UserModuleRelMap : IEntityTypeConfiguration<UserModuleRel>
    {
        public void Configure(EntityTypeBuilder<UserModuleRel> builder)
        {
            builder.ToTable("UserModuleRel");

            builder.HasKey(d => d.UMID);
            builder.Property(d => d.UMID).HasColumnType("varchar(36)");
            builder.Property(d => d.Status).HasColumnType("char(1)").IsRequired();


            builder.HasOne(d => d.UserModule).WithMany(d => d.UserModuleRel).HasConstraintName("FK_Module_Role_MId").HasForeignKey(d => d.MId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(d => d.UserRole).WithMany(d => d.UserModuleRel).HasConstraintName("FK_Role_Module_RId").HasForeignKey(d => d.RId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
