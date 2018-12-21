using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Sklub.Models
{
    // Sie können Profildaten für den Benutzer hinzufügen, indem Sie der ApplicationUser-Klasse weitere Eigenschaften hinzufügen. Weitere Informationen finden Sie unter https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Member> Members { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Mutation> Mutations { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Training> Trainings { get; set; }
    }
}