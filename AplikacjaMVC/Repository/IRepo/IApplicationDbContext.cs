using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<EventCategory> EventCategories { get; set; }
        DbSet<Announcement> Annoucements { get; set; }

        DbEntityEntry Entry(object entity); 

        int SaveChanges();
        Database Database { get; }
    }
}
