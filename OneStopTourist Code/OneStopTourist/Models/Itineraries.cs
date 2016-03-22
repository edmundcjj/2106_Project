using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneStopTourist.Models
{
    public class Itineraries
    {
        [Key]
        public int Iid { get; set; }

        public string Nickname { get; set; }
        public string Content { get; set; }
    }

    public class Itineraries_has_Reviews
    {
        [Key]
        //[ForeignKey("Reviews")]
        public int Rid { get; set; }

        [Required]
        //[ForeignKey("Itineraries")]
        public int Iid { get; set; }

        //public virtual Reviews Reviews { get; set; }
        //public virtual Itineraries Itineraries { get; set; }
    }

    public class ItineraryPage
    {
        public Itineraries getItinerary { get; set; }
        public Reviews getReview { get; set; }
        public Reviews getItineraryReview { get; set; }
    }
}