using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;

namespace Repository.IRepo
{
    public interface IAnnoucementRepo
    {
        IQueryable<Announcement> GetAnnoucements();

        void AddAnnoucement(Announcement ann);
    }
}
