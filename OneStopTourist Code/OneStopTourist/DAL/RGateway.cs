using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneStopTourist.Models;

namespace OneStopTourist.DAL
{
    public class RGateway: DataGateway<Review>
    {
        public IQueryable<Review> getAttractionReview(int? id)
        {
            var reviewModel = from x in db.Reviews
                              where (x.Aid == id)
                              select x;
            return reviewModel;
        }

        public IQueryable<Review> getServiceReview(int? id)
        {
            var reviewModel = from x in db.Reviews
                              where (x.Sid == id)
                              select x;
            return reviewModel;
        }
    }
}