using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OneStopTourist.Models;

namespace OneStopTourist.DAL
{
    public class OneStopTouristContext : DbContext
    {
        public OneStopTouristContext() : base("One Stop Tourist")
        {

        }
        
        public DbSet<Attractions> Attractions { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Itineraries> Itineraries { get; set; }
        public DbSet<Personal_Itineraries> Personal_Itineraries { get; set; }

        public DbSet<Attractions_has_Reviews> AttractionReviews { get; set; }
        public DbSet<Services_has_Reviews> ServiceReviews { get; set; }
        public DbSet<Itineraries_has_Reviews> ItineraryReviews { get; set; }
    }
}