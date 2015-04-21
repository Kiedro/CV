using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class EventCategory
    {
        public EventCategory()
        {
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int EventID { get; set; }

        public virtual Category Category { get; set; }
        public virtual Event Event { get; set; }
    }
}