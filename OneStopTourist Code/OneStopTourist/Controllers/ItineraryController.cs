using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneStopTourist.Models;
using OneStopTourist.DAL;

namespace OneStopTourist.Controllers
{
    public class ItineraryController : Controller
    {
        private ItineraryGateway iGateway = new ItineraryGateway();
        private ReviewGateway rGateWay = new ReviewGateway();
        private AttractionGateway aGateWay = new AttractionGateway();
        private ServiceGateway sGateway = new ServiceGateway();
        private DataGateway<Itineraries_has_Reviews> irGateWay = new DataGateway<Itineraries_has_Reviews>();

        // GET: All Itineraries
        public ActionResult Itineraries()
        {
            List<HomePage> allList = new List<HomePage>();
            List<HomePage> returningList = new List<HomePage>();

            var allItineraries = iGateway.getAllItineraries();
            using (var itineraryList = allItineraries.GetEnumerator())
            {
                while (itineraryList.MoveNext())
                {
                    var itineraries = itineraryList.Current;

                    HomePage item = new HomePage();
                    item.getItinerary = itineraries;
                    allList.Add(item);
                }
            }

            foreach (HomePage obj in allList)
            {
                returningList.Add(obj);
                string spotID = obj.getItinerary.Content;
                string[] spots = spotID.Split(',');
                int stringCount = spots.Count();
                foreach (string contentList in spots.Take(stringCount-1))
                {
                    string getIdentificationNo = contentList.Substring(2, contentList.Length - 2);
                    int identificationNo = Convert.ToInt32(getIdentificationNo);
                    if (contentList.Substring(0, 2) == "A-")
                    {
                        HomePage item = new HomePage();
                        Attractions attractionSpot = aGateWay.SelectById(identificationNo);
                        item.getAttraction = attractionSpot;
                        returningList.Add(item);
                    }
                    else if (contentList.Substring(0, 2) == "S-")
                    {
                        HomePage item = new HomePage();
                        Services serviceSpot = sGateway.SelectById(identificationNo);
                        returningList.Add(item);
                    }
                }
            }

            return View(returningList);
        }

        // GET: Recommended Itineraries
        public ActionResult RecommendedItineraries()
        {
            List<HomePage> allList = new List<HomePage>();
            List<HomePage> returningList = new List<HomePage>();

            var recoItineraries = iGateway.getRecommendedItineraries();
            using (var itineraryList = recoItineraries.GetEnumerator())
            {
                while (itineraryList.MoveNext())
                {
                    var itineraries = itineraryList.Current;

                    HomePage item = new HomePage();
                    item.getItinerary = itineraries;
                    allList.Add(item);
                }
            }

            foreach (HomePage obj in allList)
            {
                returningList.Add(obj);
                string spotID = obj.getItinerary.Content;
                string[] spots = spotID.Split(',');
                int stringCount = spots.Count();
                foreach (string contentList in spots.Take(stringCount - 1))
                {
                    string getIdentificationNo = contentList.Substring(2, contentList.Length - 2);
                    int identificationNo = Convert.ToInt32(getIdentificationNo);
                    if (contentList.Substring(0, 2) == "A-")
                    {
                        HomePage item = new HomePage();
                        Attractions attractionSpot = aGateWay.SelectById(identificationNo);
                        item.getAttraction = attractionSpot;
                        returningList.Add(item);
                    }
                    else if (contentList.Substring(0, 2) == "S-")
                    {
                        HomePage item = new HomePage();
                        Services serviceSpot = sGateway.SelectById(identificationNo);
                        returningList.Add(item);
                    }
                }
            }

            return View(returningList);
        }

        // GET: Itinerary Details
        public ActionResult ViewItinerary(int? id)
        {
            var reviewModel = rGateWay.getItineraryReview(id);

            HomePage viewItem = new HomePage();
            viewItem.getItinerary = iGateway.SelectById(id);
            
            //Create a list to store both the itinerary details and all of its reviews
            List<HomePage> reviewList = new List<HomePage>();
            //First review is stored in the first element of reviewList with the attraction details
            reviewList.Add(viewItem);

            string itineraryString = viewItem.getItinerary.Content;

            string[] itineraryList = itineraryString.Split(',');
            int getSpotsCount = itineraryList.Count();
            foreach (string spots in itineraryList.Take(getSpotsCount - 1))
            {
                HomePage itinerarySpots = new HomePage();
                string spotIDstring = spots.Substring(2, spots.Length - 2);
                int spotID = Convert.ToInt32(spotIDstring);
                if (spots.Substring(0, 2) == "A-")
                {
                    Attractions attractionSpot = aGateWay.SelectById(spotID);
                    itinerarySpots.getAttraction = attractionSpot;
                    reviewList.Add(itinerarySpots);
                }
                else if (spots.Substring(0, 2) == "S-")
                {
                    Services serviceSpot = sGateway.SelectById(spotID);
                    itinerarySpots.getService = serviceSpot;
                    reviewList.Add(itinerarySpots);
                }
            }

            if (reviewModel.Any())
            {
                foreach (Reviews item in reviewModel)
                {
                    HomePage chgItem = new HomePage();
                    chgItem.getReview = item;
                    reviewList.Add(chgItem);
                }
            }
            return View(reviewList);
        }

        // POST: Itinerary/ViewItinerary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewItinerary(int id)
        {
            Reviews review = new Reviews();
            Itineraries_has_Reviews itineraryReview = new Itineraries_has_Reviews();
            if (Request.Form["rating"] != null)
            {
                string givenRatings = Request.Form["rating"].ToString();
                string givenNickname = Request.Form["nickname"].ToString();
                string givenContent = Request.Form["content"].ToString();
                review.Ratings = givenRatings;
                review.Nickname = givenNickname;
                review.Content = givenContent;
                review.ReviewDate = DateTime.Now;
            }
            rGateWay.Insert(review);

            itineraryReview.Rid = review.Rid;
            itineraryReview.Iid = id;
            irGateWay.Insert(itineraryReview);
            return RedirectToAction("Itineraries", id);
        }

        public ActionResult UseItinerary(int? id)
        {
            List<HomePage> getItinerary = new List<HomePage>();
            HomePage viewItem = new HomePage();
            viewItem.getItinerary = iGateway.SelectById(id);

            string itineraryString = viewItem.getItinerary.Content;

            string[] itineraryList = itineraryString.Split(',');
            int getSpotsCount = itineraryList.Count();
            foreach (string spots in itineraryList.Take(getSpotsCount - 1))
            {
                HomePage itinerarySpots = new HomePage();
                string spotIDstring = spots.Substring(2, spots.Length - 2);
                int spotID = Convert.ToInt32(spotIDstring);
                if (spots.Substring(0, 2) == "A-")
                {
                    Attractions attractionSpot = aGateWay.SelectById(spotID);
                    itinerarySpots.getAttraction = attractionSpot;
                    getItinerary.Add(itinerarySpots);
                }
                else if (spots.Substring(0, 2) == "S-")
                {
                    Services serviceSpot = sGateway.SelectById(spotID);
                    itinerarySpots.getService = serviceSpot;
                    getItinerary.Add(itinerarySpots);
                }
            }

            Session["myItinerary"] = getItinerary;


            return RedirectToAction("../Home/Index");
        }
    }
}
