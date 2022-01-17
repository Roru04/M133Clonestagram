namespace Clonestagram.Migrations
{
    using Clonestagram.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Clonestagram.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Clonestagram.Models.ApplicationDbContext";
        }

        protected override void Seed(Clonestagram.Models.ApplicationDbContext context)
        {
            ApplicationUser admin =
            context.Users.SingleOrDefault(u => u.Email == "a@b.c");
            IdentityRole adminRole =
            context.Roles.Single(r => r.Name == Role.Administrator.ToString());
            if (admin != null && !admin.Roles.Any())
            {
                admin.Roles.Add(new IdentityUserRole { RoleId = adminRole.Id });
            }
        }
    }
}
