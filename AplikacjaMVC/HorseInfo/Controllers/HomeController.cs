using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Repository.IRepo;
using Repository.Models;
using Repository.Models.Views;

namespace HorseInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventRepo _repo;

        public HomeController(IEventRepo repo)
        {
            _repo = repo;
        }

        //TODO dodać sortowanie według dat itp
        public ActionResult Index(int? page, int? categoryId)
        {
            int currentPage = page ?? 1;
            const int onPage = 5;
            List<Event> events;
            if (categoryId.HasValue)
            {
                events = _repo.GetEventsFromCategory(categoryId.Value).Include("User").Include("User.Company").AsNoTracking().ToList();
            }
            else
                events = _repo.GetEvents().OrderByDescending(d => d.Date).Include("User").Include("User.Company").AsNoTracking().ToList();

            return View(events.ToPagedList(currentPage, onPage));
        }
    }
}