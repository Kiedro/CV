using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IEventRepo
    {
        IQueryable<Event> GetEvents();

        Event GetEventById(int id);
        void DeleteEvent(int id);

        void AddEvent(Event @event);

        void SaveChanges();

        void Update(Event @event);

        IQueryable<Event> GetPage(int? page = 1, int? pageSize = 10);

        IQueryable<Announcement> GetAnnoucements();

        void AddAnnoucement(Announcement ann);

        #region Categories
        IQueryable<Category> GetCategories();
        IQueryable<Event> GetEventsFromCategory(int id);

        string CategoryName(int id);
        #endregion
    }
}
