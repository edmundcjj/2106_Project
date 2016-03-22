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
        
        public string Content { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Nickname must be at least {2} characters long.", MinimumLength = 2)]
        public string Nickname { get; set; }

        public string Pin { get; set; }
    }
}