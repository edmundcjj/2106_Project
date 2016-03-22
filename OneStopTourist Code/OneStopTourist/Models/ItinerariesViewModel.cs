using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OneStopTourist.Models
{
    public class ItinerariesViewModel
    {
        public Itineraries getItineraries { get; set; }
        public Personal_Itineraries getPersonal_Itineraries { get; set; }
    }
}