﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneStopTourist.Models;

namespace OneStopTourist.DAL
{
    public class AttractionGateway : DataGateway<Attractions>
    {
        public IQueryable<string> getCategories()
        {
            var categories = (from x in db.Attractions
                              select x.Category).Distinct();

            return categories;
        }

        public IQueryable<Attractions> getAttractionsByCat(string category)
        {
            var categories = from x in db.Attractions
                             orderby x.Name
                             where (x.Category == category) || (category == null) || (category == "")
                             select x;

            return categories;
        }

        public IQueryable<Attractions> getRecommendedAttractions()
        {
            var reviewModel = (from x in db.Attractions
                              join y in db.AttractionReviews on x.Aid equals y.Aid
                              join z in db.Reviews on y.Rid equals z.Rid
                              orderby z.Ratings ascending
                              select x).GroupBy(a => a.Aid).Select(b => b.FirstOrDefault());

            return reviewModel;
        }
    }
}