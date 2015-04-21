using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Category
    {
        public Category()
        {
            this.EventCategory = new HashSet<EventCategory>();
        }

        [Key]
        [Display(Name = "ID kategorii")]
        public int Id {get;set;}

        [Display(Name="Nazwa kategorii")]
        [Required]
        public string Name {get; set;}

        [Display(Name = "Id rodzica")]
        [Required]
        public int ParentId { get; set; }

        #region SEO
        
        [Display(Name = "Tytuł w google")]
        [MaxLength(72)]
        public string MetaTitle { get; set; }

        [Display(Name = "Opis strony w google")]
        [MaxLength(160)]
        public string MetaDescription { get; set; }

        [Display(Name = "Słowa kluczowe w google")]
        [MaxLength(160)]
        public string MetaWords { get; set; }

        [Display(Name = "Treść strony")]
        [MaxLength(500)]
        public string Content { get; set; }

        #endregion

        public ICollection<EventCategory> EventCategory {get;set;}
    }
}