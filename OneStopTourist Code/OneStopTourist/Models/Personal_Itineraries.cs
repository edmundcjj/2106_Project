using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneStopTourist.Models
{
    public class Personal_Itineraries
    {
        [Key]
        public int PIid { get; set; }
        
        public string Nickname { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
    }
}