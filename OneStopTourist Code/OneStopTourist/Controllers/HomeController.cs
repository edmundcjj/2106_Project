using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneStopTourist.Models;
using OneStopTourist.DAL;

namespace OneStopTourist.Controllers
{
    public class HomeController : Controller
    {
        private DataGateway<Attractions> aDGateWay = new DataGateway<Attractions>();
        private DataGateway<Services> sDGateWay = new DataGateway<Services>();
        private AttractionGateway aGateway = new AttractionGateway();
        private ServiceGateway sGateway = new ServiceGateway();

        public ActionResult Index()
        {
            List<HomePage> allList = new List<HomePage>();

            var bestAttractions = aGateway.getRecommendedAttractions();
            var bestServices = sGateway.getRecommendedServices();
                using (var attractionsList = bestAttractions.GetEnumerator())
                using (var servicesList = bestServices.GetEnumerator())
                {
                    while (attractionsList.MoveNext() && servicesList.MoveNext())
                    {
                        var attractions = attractionsList.Current;
                        var services = servicesList.Current;

                        // Load both objects with ratings into the list
                        HomePage item = new HomePage();
                        item.getAttraction = attractions;
                        item.getService = services;
                        allList.Add(item);
                    }
                }
            
            //To fill up the list with random recommendations if list have less than 5 objects
            if (allList.Count() < 5)
            {
                while (allList.Count() < 5)
                {
                    Random x = new Random();
                    int IdNo = x.Next(1, aDGateWay.SelectAll().Count());
                    HomePage item = new HomePage();
                    item.getAttraction = aDGateWay.SelectById(IdNo);
                    item.getService = sDGateWay.SelectById(IdNo);
                    allList.Add(item);
                }
            }

            return View(allList);
        }

        public ActionResult Itinerary()
        {
            return View();
        }

        public ActionResult saveItinerary()
        {
            return View();
        }
    }
}