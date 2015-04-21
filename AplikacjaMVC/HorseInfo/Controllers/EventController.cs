using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Repository.IRepo;
using Repository.Models;
using Repository.Models.Views;

namespace HorseInfo.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepo _repo;

        public EventController(IEventRepo repo)
        {
            _repo = repo;
        }

        // GET: Event/Id
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index", "Home");

            Event @event = _repo.GetEventById(id.Value);
            if (@event == null)
                return RedirectToAction("Index", "Home");

            EventDetailsViewModel viewModel = new EventDetailsViewModel
            {
                ev = @event,
                Annoucements = _repo.GetAnnoucements().AsNoTracking<Announcement>()
                                     .Where(o => o.EventId == id.Value).ToList<Announcement>()
            };
            return View(viewModel);
        }

        public ActionResult Organizer(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index", "Home");

            Event @event = _repo.GetEventById(id.Value);
            if (@event == null)
                return RedirectToAction("Index", "Home");

            return View(@event);
        }

        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description")] Event @event)
        {
            if (!ModelState.IsValid)
                return View(@event);

            @event.UserId = User.Identity.GetUserId();
            @event.CreationDate = DateTime.Now;
            @event.Date = DateTime.Now.AddDays(5);
            try
            {
                _repo.AddEvent(@event);
                _repo.SaveChanges();
                //TODO return RedirectToAction("MyEvents"); stworzyć tę akcję i faktycznie do niej przekierować
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View(@event);
            }
        }
        
        public ActionResult Map(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index", "Home");

            Event @event = _repo.GetEventById(id.Value);
            if (@event == null)
                return RedirectToAction("Index", "Home");

            return View(@event);
        }
    }
}