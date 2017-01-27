using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PortalUslug.Models
{

    public class PortalUslugContext : IdentityDbContext
    {
        public PortalUslugContext() : base("PortalUslugConnection"){}

        public static PortalUslugContext Create()
        {
            return new PortalUslugContext();
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentCategory> CommentCategories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //wylacza tworzenie tabel z liczba mnoga
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //wylacza kaskadowe usuwanie powiazan
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}