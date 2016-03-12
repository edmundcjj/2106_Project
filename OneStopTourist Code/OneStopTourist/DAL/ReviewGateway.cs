using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneStopTourist.Models;

namespace OneStopTourist.DAL
{
    public class ReviewGateway: DataGateway<Reviews>
    {
        public IQueryable<Reviews> getAttractionReview(int? id)
        {
            //Get list of reviews of a particular Attraction, Aid
            var reviewModel = from x in db.Reviews
                            join y in db.AttractionReviews on x.Rid equals y.Rid
                              where (y.Aid == id)
                              select x;
            
            return reviewModel;
        }

        public IQueryable<Reviews> getServiceReview(int? id)
        {
            var reviewModel = from x in db.Reviews
                            join y in db.ServiceReviews on x.Rid equals y.Rid
                            where (y.Sid == id)
                            select x;
            
            return reviewModel;
        }
    }
}