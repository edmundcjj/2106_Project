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
            
            List<HomePage> sessionItinerary = (List<HomePage>) Session["myItinerary"];

            if (sessionItinerary != null)
            {
                foreach (HomePage item in sessionItinerary)
                {
                    allList.Add(item);
                }
            }

            return View(allList);
        }

        public ActionResult AddAttraction(int? id)
        {
            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getAttraction = aGateway.SelectById(id);
            if (sessionItinerary == null)
            {
                sessionItinerary = new List<HomePage>();
                sessionItinerary.Add(item);
            }
            else {
                sessionItinerary.Add(item);
            }

            Session["myItinerary"] = sessionItinerary;

            return RedirectToAction("Index");
        }

        public ActionResult AddService(int? id)
        {
            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getService = sGateway.SelectById(id);

            if (sessionItinerary == null)
            {
                sessionItinerary = new List<HomePage>();
                sessionItinerary.Add(item);
            }
            else
            {
                sessionItinerary.Add(item);
            }

            Session["myItinerary"] = sessionItinerary;

            return RedirectToAction("Index");
        }

        public ActionResult RemoveAttraction(int? id)
        {
            int selectedindex = 0;
            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getAttraction = aGateway.SelectById(id);
            if (sessionItinerary != null)
            {
                for (int i = 0; i < sessionItinerary.Count; i++)
                {
                    var something = sessionItinerary[i];

                    if (sessionItinerary[i].getAttraction != null)
                    {
                        if ((sessionItinerary[i].getAttraction.Name).Equals(item.getAttraction.Name))
                        {
                            selectedindex = i;
                        }
                    }

                }

                sessionItinerary.RemoveAt(selectedindex);
            }

            //If it has nothing, deem the session null
            if (sessionItinerary.Count() == 0)
            {
                Session["myItinerary"] = null;
            }
            else {
                Session["myItinerary"] = sessionItinerary;
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveService(int? id)
        {
            int selectedindex = 0;
            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getService = sGateway.SelectById(id);
            if (sessionItinerary != null)
            {
                for (int i = 0; i < sessionItinerary.Count; i++)
                {
                    var something = sessionItinerary[i];
                    
                    if (sessionItinerary[i].getService != null)
                    {
                        if ((sessionItinerary[i].getService.Name).Equals(item.getService.Name))
                        {
                            selectedindex = i;
                        }
                    }

                }

                sessionItinerary.RemoveAt(selectedindex);
            }

            //If it has nothing, deem the session null
            if (sessionItinerary.Count() == 0)
            {
                Session["myItinerary"] = null;
            }
            else {
                Session["myItinerary"] = sessionItinerary;
            }

            return RedirectToAction("Index");
        }

        public ActionResult MoveAttractionDown(int? id)
        {
            int selectedindex = 0;

            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getAttraction = aGateway.SelectById(id);

            if (sessionItinerary != null)
            {
                for (int i = 0; i < sessionItinerary.Count; i++)
                {
                    var something = sessionItinerary[i];

                    if (sessionItinerary[i].getAttraction != null)
                    {
                        if ((sessionItinerary[i].getAttraction.Name).Equals(item.getAttraction.Name))
                        {
                            selectedindex = i;
                        }
                    }

                }

                if ((sessionItinerary.Count-1 != 0) && (selectedindex != sessionItinerary.Count-1))
                {
                    var x = sessionItinerary[selectedindex];
                    var y = sessionItinerary[selectedindex + 1];

                    sessionItinerary[selectedindex] = y;
                    sessionItinerary[selectedindex + 1] = x;
                }
            }

            Session["myItinerary"] = sessionItinerary;

            return RedirectToAction("Index");
        }

        public ActionResult MoveAttractionUp(int? id)
        {
            int selectedindex = 0;

            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getAttraction = aGateway.SelectById(id);

            if (sessionItinerary != null)
            {
                for (int i = 0; i < sessionItinerary.Count; i++)
                {
                    var something = sessionItinerary[i];

                    if (sessionItinerary[i].getAttraction != null)
                    {
                        if ((sessionItinerary[i].getAttraction.Name).Equals(item.getAttraction.Name))
                        {
                            selectedindex = i;
                        }
                    }

                }
                
                if ((sessionItinerary.Count - 1 != 0) && (selectedindex != 0))
                {
                    var x = sessionItinerary[selectedindex];
                    var y = sessionItinerary[selectedindex - 1];

                    sessionItinerary[selectedindex] = y;
                    sessionItinerary[selectedindex - 1] = x;
                }
            }

            Session["myItinerary"] = sessionItinerary;

            return RedirectToAction("Index");
        }

        public ActionResult MoveServiceDown(int? id)
        {
            int selectedindex = 0;

            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getService = sGateway.SelectById(id);

            if (sessionItinerary != null)
            {
                for (int i = 0; i < sessionItinerary.Count; i++)
                {
                    var something = sessionItinerary[i];

                    if (sessionItinerary[i].getService != null)
                    { 
                        if ((sessionItinerary[i].getService.Name).Equals(item.getService.Name))
                        {
                            selectedindex = i;
                        }
                    }
                }

                if ((sessionItinerary.Count - 1 != 0) && (selectedindex != sessionItinerary.Count-1))
                {
                    var x = sessionItinerary[selectedindex];
                    var y = sessionItinerary[selectedindex + 1];

                    sessionItinerary[selectedindex] = y;
                    sessionItinerary[selectedindex + 1] = x;
                }
            }

            Session["myItinerary"] = sessionItinerary;

            return RedirectToAction("Index");
        }

        public ActionResult MoveServiceUp(int? id)
        {
            int selectedindex = 0;

            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            HomePage item = new HomePage();
            item.getService = sGateway.SelectById(id);

            if (sessionItinerary != null)
            {
                for (int i = 0; i < sessionItinerary.Count; i++)
                {
                    var something = sessionItinerary[i];

                    if (sessionItinerary[i].getService != null)
                    {
                        if ((sessionItinerary[i].getService.Name).Equals(item.getService.Name))
                        {
                            selectedindex = i;
                        }
                    }
                }

                if ((sessionItinerary.Count - 1 != 0) && (selectedindex != 0))
                {
                    var x = sessionItinerary[selectedindex];
                    var y = sessionItinerary[selectedindex - 1];

                    sessionItinerary[selectedindex] = y;
                    sessionItinerary[selectedindex - 1] = x;
                }
            }

            Session["myItinerary"] = sessionItinerary;

            return RedirectToAction("Index");
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