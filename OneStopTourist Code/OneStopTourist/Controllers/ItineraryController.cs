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

        // GET: All Itineraries
        public ActionResult Itineraries()
        {
            return View(iGateway.getAllItineraries());
        }

        // GET: Recommended Itineraries
        public ActionResult RecommendedItineraries()
        {
            return View(iGateway.getRecommendedItineraries());
        }

        // GET: Itinerary Details
        public ActionResult ViewItinerary(int? id)
        {
            var reviewModel = rGateWay.getItineraryReview(id);

            ItineraryPage viewItem = new ItineraryPage();
            viewItem.getItinerary = iGateway.SelectById(id);

            if (!reviewModel.Any())
            {
                List<ItineraryPage> reviewPage = new List<ItineraryPage>();
                reviewPage.Add(viewItem);

                return View(reviewPage);
            }
            else {
                //Create a list to store both the itinerary details and all of its reviews
                List<ItineraryPage> reviewList = new List<ItineraryPage>();
                //First review is stored in the first element of reviewList with the attraction details
                reviewList.Add(viewItem);
                foreach (Reviews item in reviewModel)
                {
                    ItineraryPage chgItem = new ItineraryPage();
                    chgItem.getReview = item;
                    reviewList.Add(chgItem);
                }
                return View(reviewList);
            }
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
            return RedirectToAction("Itineraries", id);
        }







        // GET: Itinerary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Itinerary/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Itinerary/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Itinerary/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Itinerary/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Itinerary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Itinerary/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
