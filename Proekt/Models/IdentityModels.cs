using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Proekt.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserDetailsModel> UserDetails { get; set; }
        public DbSet<ActorRoleModel> ActorRoles { get; set; }
        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<ActorModel> Actors { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<ListModel> Lists { get; set; }
        public DbSet<CollectionModel> Collections { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieModel>().
                HasMany(m => m.Companies).
                WithMany();

            modelBuilder.Entity<ActorModel>().
                HasMany(a => a.Movies).
                WithMany();

            modelBuilder.Entity<UserDetailsModel>().
                HasMany(u => u.Favorites).
                WithMany();
        }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}