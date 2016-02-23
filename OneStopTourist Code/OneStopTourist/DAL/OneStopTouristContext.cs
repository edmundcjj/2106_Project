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
        public OneStopTouristContext() : base("2106_Project")
        {

        }
        
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
    }
}