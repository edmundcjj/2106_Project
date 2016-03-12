using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneStopTourist.Models;

namespace OneStopTourist.DAL
{
    public class ServiceGateway : DataGateway<Services>
    {
        public IQueryable<string> getCategories()
        {
            var categories = (from x in db.Services
                              select x.Category).Distinct();

            return categories;
        }

        public IQueryable<Services> getServicesByCat(string category)
        {
            var categories = from x in db.Services
                             orderby x.Name
                             where (x.Category == category) || (category == null) || (category == "")
                             select x;

            return categories;
        }
    }
}