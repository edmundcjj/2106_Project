using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneStopTourist.Models;

namespace OneStopTourist.DAL
{
    public class AGateway : DataGateway<Attraction>
    {
        public IQueryable<string> getCategories()
        {
            var categories = (from x in db.Attractions
                              select x.Category).Distinct();

            return categories;
        }

        public IQueryable<Attraction> getAttractionsByCat(string category)
        {
            var categories = from x in db.Attractions
                             orderby x.Name
                             where (x.Category == category) || (category == null) || (category == "")
                             select x;

            return categories;
        }
    }
}