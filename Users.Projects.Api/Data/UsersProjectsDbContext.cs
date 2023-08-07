using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Users.Projects.Api.Data.Models;
using static Users.Projects.Api.Common.Constants.Properties;

namespace Users.Projects.Api.Data
{
    public class UsersProjectsDbContext : DbContext
    {
        public UsersProjectsDbContext(DbContextOptions<UsersProjectsDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfoRules();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInfoRules();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>();
            builder.Entity<Project>();
            builder.Entity<TimeLog>();
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in changedEntries)
            {
                PropertyInfo? entryProperty = null;

                if (entry.State == EntityState.Added)
                {
                    entryProperty = entry.Entity.GetType().GetProperty(DATE_ADDED_PROPERTY_STRING_LITERAL);
                }

                if (entryProperty != null)
                {
                    entryProperty.SetValue(entry.Entity, DateTime.UtcNow);
                }
            }
        }
    }
}
