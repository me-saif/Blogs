using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blogs.Models
{
    public class BlogDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogDbContext(DbContextOptions<BlogDbContext> options, IHttpContextAccessor httpContextAccessor) :
          base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public BlogDbContext()
        {
        }

        public DbSet<Category> Categories { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var now = DateTime.Now.Date;
            var currentUser = !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.User?.Identity?.Name) ? _httpContextAccessor.HttpContext.User.Identity.Name
            : "Anonymous";
            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is BaseEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDate = now;
                            entity.CreatedBy = currentUser;
                            Entry(entity).Property(x => x.UpdatedBy).IsModified = false;
                            Entry(entity).Property(x => x.UpdatedDate).IsModified = false;
                            break;

                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                            Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                            entity.UpdatedDate = now;
                            entity.UpdatedBy = currentUser;
                            break;
                    }
                }
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}