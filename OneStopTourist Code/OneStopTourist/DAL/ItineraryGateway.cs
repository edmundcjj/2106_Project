using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneStopTourist.Models;

namespace OneStopTourist.DAL
{
    public class ItineraryGateway: DataGateway<Itineraries>
    {
        public IQueryable<Itineraries> getRecommendedItineraries()
        {
            var recommendedItinerary = (from x in db.Itineraries
                                        join y in db.ItineraryReviews on x.Iid equals y.Iid
                                        join z in db.Reviews on y.Rid equals z.Rid
                                        orderby z.Ratings ascending
                                        select x);

            return recommendedItinerary;
        }
    }
}