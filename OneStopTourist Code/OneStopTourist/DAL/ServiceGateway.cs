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

        public IQueryable<Services> getRecommendedServices()
        {
            var reviewModel = (from x in db.Services
                              join y in db.ServiceReviews on x.Sid equals y.Sid
                              join z in db.Reviews on y.Rid equals z.Rid
                              orderby z.Ratings ascending
                              select x).GroupBy(a => a.Sid).Select(b => b.FirstOrDefault()); ;

            return reviewModel;
        }
    }
}