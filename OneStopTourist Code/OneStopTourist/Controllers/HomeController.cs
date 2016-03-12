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
        private DataGateway<Attractions> aGateWay = new DataGateway<Attractions>();
        private DataGateway<Services> sGateWay = new DataGateway<Services>();

        public ActionResult Index()
        {
            List<HomePage> allList = new List<HomePage>();
            int allCount = 5;
            while (allCount != 0)
            {
                HomePage item = new HomePage();
                item.getAttraction = aGateWay.SelectById(allCount);
                item.getService = sGateWay.SelectById(allCount);
                allList.Add(item);
                allCount = allCount - 1;
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