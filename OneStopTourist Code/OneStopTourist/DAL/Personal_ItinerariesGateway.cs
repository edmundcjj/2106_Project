using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneStopTourist.Models;
using System.Collections;

namespace OneStopTourist.DAL
{
    public class Personal_ItinerariesGateway : DataGateway<Personal_Itineraries>
    {
        //use for generate random pin that is unique
        public int SelectHighestPIid()
        {
            //find highest PIid value
            int maxPIid = (from x in db.Personal_Itineraries
                           select x.PIid).Max();
            return maxPIid;
        }

        //insert into Itineraries table and delete from Personal_Itineraries table if it exist
        public int getItinerariesPId(string nickname, string content)
        {
            //check whether itineraries exist
            if (db.Personal_Itineraries.Where(x => x.Nickname == nickname && x.Content == content).Any())
            {
                //retrieve PIid if it exist
                int PIid = (from u in db.Personal_Itineraries
                            where (u.Nickname == nickname) && (u.Content == content)
                            select u.PIid).FirstOrDefault();

                return PIid;
            }
            return - 1;
        }

        //retrieve personal itinerary
        public IQueryable<Personal_Itineraries> getPersonal_Itineraries(string nickname, string pin)
        {
            var content = from x in db.Personal_Itineraries
                             where (x.Nickname == nickname) && (x.Pin == pin)
                             select x;

            return content;
        }

        public IQueryable<Personal_Itineraries> checkLogin(string nickname)
        {
            var content = from x in db.Personal_Itineraries
                          where (x.Nickname == nickname)
                          select x;

            return content;
        }

        // Check for duplicate nicknames in database
        public Boolean check_duplicate_nickname(string nickname)
        {
            return (db.Personal_Itineraries.Any(x => x.Nickname == nickname));
        }
    }
}