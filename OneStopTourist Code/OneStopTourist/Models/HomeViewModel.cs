using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneStopTourist.Models
{
    public class Attraction
    {
        [Key]
        public int Aid { get; set; }

        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    public class Service
    {
        [Key]
        public int Sid { get; set; }

        [Required]
        public string Name { get; set; }
        public string Contact_Details { get; set; }
        public string Category { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    public class Review
    {
        [Key]
        public int Rid { get; set; }

        [Required]
        public int Aid { get; set; }
        public int Sid { get; set; }
        public int Iid { get; set; }

        public string Nickname { get; set; }
        public int Ratings { get; set; }
        public string Content { get; set; }
    }

    public class HomePage
    {
        public Attraction getAttraction { get; set; }
        public Service getService { get; set; }
        public Review getReview { get; set; }
    }
}