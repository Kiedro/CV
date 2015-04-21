using Repository.IRepo;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Repository.Repo
{
    public class EventRepo : IEventRepo
    {
        private readonly IApplicationDbContext _db;

        public EventRepo(IApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Event> GetEvents()
        {
            _db.Database.Log = message => Trace.WriteLine(message);
            return _db.Events.AsNoTracking();
        }

        //TODO napisać to lepiej. Aktualna wersja pobiera wszystkie a następnie wyszukuje zadany event.
        // Inaczej pobierany był event wraz z danymi User, ale nie User.Company
        public Event GetEventById(int id)
        {
            //Event @event = _db.Events.Find(id);
            List<Event> @event = _db.Events
                .Where(e => e.Id == id).Include("User").Include("User.Company").AsNoTracking().ToList();
                
            return @event[0];
        }


        public void DeleteEvent(int id)
        {
            DeleteRelatedEventCategory(id);

            Event @event = _db.Events.Find(id);

            _db.Events.Remove(@event);
        }

        private void DeleteRelatedEventCategory(int idEvent)
        {
            var list = _db.EventCategories.Where(o => o.EventID == idEvent);

            foreach (var el in list)
            {
                _db.EventCategories.Remove(el);
            }
        }


        public void SaveChanges()
        {
            _db.SaveChanges();
        }


        public void AddEvent(Event @event)
        {
            _db.Events.Add(@event);
        }


        public void Update(Event @event)
        {
            _db.Entry(@event).State = EntityState.Modified;
        }


        public IQueryable<Event> GetPage(int? page = 1, int? pageSize = 10)
        {
            var events = _db.Events.OrderByDescending(o => o.Date)
                                    .Skip((page.Value - 1) * pageSize.Value)
                                    .Take(pageSize.Value);
            return events;
        }


        public IQueryable<Announcement> GetAnnoucements()
        {
            //todo
            var annoucements = _db.Annoucements.AsNoTracking<Announcement>();
            return annoucements;
        }

        public void AddAnnoucement(Announcement ann)
        {
            throw new NotImplementedException();
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
            string name = _db.Categories.Find(id).Name;
            return name;
        }
    }
}