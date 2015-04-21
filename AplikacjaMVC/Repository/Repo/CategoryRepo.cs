using Repository.IRepo;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Repository.Repo
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly IApplicationDbContext _db;

        public CategoryRepo(IApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Models.Category> GetCategories()
        {
            _db.Database.Log = message => Trace.WriteLine(message);
            return _db.Categories.AsNoTracking();
        }


        public IQueryable<Event> GetEventsFromCategory(int id)
        {
            _db.Database.Log = message => Trace.WriteLine(message);
            var events = from o in _db.Events
                         join k in _db.EventCategories on o.Id equals k.Id
                         where k.CategoryId == id
                         select o;
            return events;
        }


        public string CategoryName(int id)
        {
            var name = _db.Categories.Find(id).Name;
            return name;
        }
    }
}