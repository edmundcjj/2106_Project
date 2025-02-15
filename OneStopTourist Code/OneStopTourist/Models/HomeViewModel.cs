﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OneStopTourist.Models
{
    public class Attractions
    {
        [Key]
        public int Aid { get; set; }
        
        public string Name { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }

        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public decimal Latitude { get; set; }
    }

    public class Services
    {
        [Key]
        public int Sid { get; set; }
        
        public string Name { get; set; }
        public string Contact_Details { get; set; }
        public string Category { get; set; }

        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public decimal Latitude { get; set; }
    }

    public class Reviews
    {
        [Key]
        public int Rid { get; set; }

        [Required]
        public string Nickname { get; set; }

        public string Ratings { get; set; }
        public string Content { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class Attractions_has_Reviews
    {
        [Key]
        //[ForeignKey("Reviews")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Rid { get; set; }

        [Required]
        //[ForeignKey("Attractions")]
        public int Aid { get; set; }

        //public virtual Reviews Reviews { get; set; }
        //public virtual Attractions Attractions { get; set; }
    }

    public class Services_has_Reviews
    {
        [Key]
        //[ForeignKey("Reviews")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Rid { get; set; }

        [Required]
        //[ForeignKey("Services")]
        public int Sid { get; set; }

        //public virtual Reviews Reviews { get; set; }
        //public virtual Services Services { get; set; }
    }

    public class Personal_Itineraries
    {
        [Key]
        public int PIid { get; set; }

        public string Content { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Nickname must be at least {2} characters long.", MinimumLength = 2)]
        public string Nickname { get; set; }

        public string Pin { get; set; }
    }

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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Rid { get; set; }

        [Required]
        //[ForeignKey("Itineraries")]
        public int Iid { get; set; }

        //public virtual Reviews Reviews { get; set; }
        //public virtual Itineraries Itineraries { get; set; }
    }

    public class HomePage
    {
        public Attractions getAttraction { get; set; }
        public Services getService { get; set; }
        public Reviews getReview { get; set; }
        public Reviews getAttractionReview { get; set; }
        public Reviews getServiceReview { get; set; }
        public Itineraries getItinerary { get; set; }
        public Reviews getItineraryReview { get; set; }
        public Personal_Itineraries getPersonalItinerary { get; set; }

        public HomePage getSessionItinerary { get; set; }
    }
}