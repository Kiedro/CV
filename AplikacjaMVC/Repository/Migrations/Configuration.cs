using System.Data;

namespace Repository.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Repository.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Repository.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Repository.Models.ApplicationDbContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();

            SeedRoles(context);
            SeedUsers(context);
            SeedEvents(context);
            SeedCategories(context);
            SeedEventCategory(context);
            SeedAnnoucements(context);
            SeedCompanies(context);
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>());

            if (!roleManager.RoleExists("Admin"))
            {
                IdentityRole role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Worker"))
            {
                IdentityRole role = new IdentityRole { Name = "Worker" };
                roleManager.Create(role);
            }
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            if (!context.Users.Any(u => u.UserName == "Admin@Admin.com"))
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Admin@Admin.com"
                };
                IdentityResult adminresult = manager.Create(user, "123456789");

                if (adminresult.Succeeded)
                {
                    manager.AddToRole(user.Id, "Admin");
                }
            }

            if (!context.Users.Any(u => u.UserName == "marek@marek.com"))
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "marek@marek.com"
                };

                IdentityResult adminresult = manager.Create(user, "123456789");

                if (adminresult.Succeeded)
                {
                    manager.AddToRole(user.Id, "Worker");
                }
            }
        }

        private static void SeedCompanies(ApplicationDbContext context)
        {
            // brak wywo³ania ToList() powoduje u¿ycie leniwego ³adowania i w konsekwecji
            // utrzymanie stale aktywnego dataReadera do contextu. Próba dodania firmy powoduje próbê 
            // otwarcia kolejnego po³¹czenia i koñczy siê wyj¹tkiem.
            var Users = context.ApplicationUsers.ToList();

            foreach (var user in Users)
            {
              
                user.Company = new Models.Company()
                {
                    //Id = user.Id,
                    PhoneNumber = "111-222-333",
                    Longitude = 51.5,
                    Lattitude = 49.5,
                    FacebookUrl = "www.facebook.com",
                    HomepageUrl = "www.google.pl",
                    CompanyName = "Oœrodek xyz",
                    CompanyAddress = "Miasto, ulica 00",
                    CompanyDescription = "dada"
                };
                //context.Set<Models.Company>().AddOrUpdate(company);
            }
            context.SaveChanges();
        }

        private static void SeedEvents(ApplicationDbContext context)
        {
            // ReSharper disable once PossibleNullReferenceException
            string idUser = context.Set<ApplicationUser>().FirstOrDefault(u => u.UserName == "Admin@Admin.com").Id;

            for (int i = 1; i <= 10; i++)
            {
                Event @event = new Event()
                {
                    Id = i,
                    UserId = idUser,
                    Description = "Opis og³oszenia " + i,
                    Title = "Tytu³ og³oszenia " + i,
                    Date = DateTime.Now.AddDays(-i),
                    CreationDate = DateTime.Now.AddDays(-2 * i)
                };
                context.Set<Event>().AddOrUpdate(@event);
            }
            context.SaveChanges();
        }

        private static void SeedCategories(ApplicationDbContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                Category cat = new Category()
                {
                    Id = i,
                    Name = "Nazwa kategorii " + i.ToString(),
                    Content = "Treœæ og³oszenia" + i.ToString(),
                    MetaTitle = "Tytu³ kategorii" + i.ToString(),
                    MetaDescription = "Opis kategorii" + i.ToString(),
                    MetaWords = "S³owa kluczowe do kategorii" + i.ToString(),
                    ParentId = i
                };
                context.Set<Category>().AddOrUpdate(cat);
            }
            context.SaveChanges();
        }

        private void SeedEventCategory(ApplicationDbContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                var eventCategory = new EventCategory()
                {
                    Id = i,
                    EventID = i / 2 + 1,
                    CategoryId = i / 2 + 1
                };
                context.Set<EventCategory>().AddOrUpdate<EventCategory>(eventCategory);
            }
            context.SaveChanges();
        }

        private void SeedAnnoucements(ApplicationDbContext context)
        {
            var idUser = context.Set<ApplicationUser>().FirstOrDefault(u => u.UserName == "Admin@Admin.com").Id;

            for (int i = 0; i < 5; i++)
            {
                Announcement ann = new Announcement()
                {
                    Id = 1,
                    EventId = 1,
                    CreationDate = DateTime.Now.AddDays(-i),
                    UserId = idUser,
                    Content = "Lorem ipsum " + i.ToString()
                };
                context.Set<Announcement>().AddOrUpdate(ann);
            }
            context.SaveChanges();
        }
    }
}
