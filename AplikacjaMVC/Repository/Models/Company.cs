using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Repository.Models
{
    public class Company
    {
        [Key, ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyDescription { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string HomepageUrl { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}