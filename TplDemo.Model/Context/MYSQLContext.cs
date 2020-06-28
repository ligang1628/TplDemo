using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TplDemo.Model.Models;

namespace TplDemo.Model.Context
{
    public class MYSQLContext : DbContext
    {
        public MYSQLContext(DbContextOptions<MYSQLContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder build)
        {
            base.OnModelCreating(build);

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(d => d.GetInterface(typeof(IEntityTypeConfiguration<>).FullName) != null);
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                build.ApplyConfiguration(configurationInstance);
            }
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserRoleRel> UserRoleRel { get; set; }
        public DbSet<UserModule> UserModule { get; set; }
        public DbSet<UserModuleRel> UserModuleRel { get; set; }

    }
}
