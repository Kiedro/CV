using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface ICategoryRepo
    {
        IQueryable<Category> GetCategories();
        IQueryable<Event> GetEventsFromCategory(int id);

        string CategoryName(int id);
    }
}
