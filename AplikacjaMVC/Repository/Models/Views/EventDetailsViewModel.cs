using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class EventDetailsViewModel
    {
        public Event ev { get; set; }
        public IList<Announcement> Annoucements { get; set; }
    }
}