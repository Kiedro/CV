using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Event
    {
        public Event()
        {
            this.EventCategory = new HashSet<EventCategory>();
            this.Annoucements = new HashSet<Announcement>();
        }

        public int Id { get; set; }

        [Display(Name = "Tytuł ogłoszenia")]
        [MaxLength(72)]
        public string Title { get; set; }

        [Display(Name = "Opis ogłoszenia")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Utworzno: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public string UserId { get; set; }


        public virtual ICollection<EventCategory> EventCategory { get; set; }
        public virtual ICollection<Announcement> Annoucements { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}