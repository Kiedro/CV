using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models.Views
{
    public class ShowEventsFromCategoryViewModel
    {
        public IList<Event> Events { get; set; }
        public string CategoryName { get; set; }
    }
}