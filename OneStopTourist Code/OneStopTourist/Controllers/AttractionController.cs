using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneStopTourist.Models;
using OneStopTourist.DAL;

namespace OneStopTourist.Controllers
{
    public class AttractionController : Controller
    {
        private DataGateway<Attractions> aGateWay = new DataGateway<Attractions>();
        private ReviewGateway rGateWay = new ReviewGateway();
        private AttractionGateway aQueryGateWay = new AttractionGateway();
        private DataGateway<Reviews> rDGateway = new DataGateway<Reviews>();
        private DataGateway<Attractions_has_Reviews> raDGateway = new DataGateway<Attractions_has_Reviews>();

        public ActionResult ViewAttraction(int? id)
        {
            var reviewModel = rGateWay.getAttractionReview(id);

            HomePage viewItem = new HomePage();
            viewItem.getAttraction = aGateWay.SelectById(id);

            if (!reviewModel.Any())
            {
                List<HomePage> attractionPage = new List<HomePage>();
                attractionPage.Add(viewItem);

                return View(attractionPage);
            }
            else {
                //Create a list to store both the attraction details and all of its reviews
                List<HomePage> reviewList = new List<HomePage>();
                //First review is stored in the first element of reviewList with the attraction details
                reviewList.Add(viewItem);
                foreach (Reviews item in reviewModel)
                {
                    HomePage chgItem = new HomePage();
                    chgItem.getReview = item;
                    reviewList.Add(chgItem);
                }
                return View(reviewList);
            }
        }

        public ActionResult Attractions(string sortOrder, string searchString, string category)
        {
            List<HomePage> allList = new List<HomePage>();
            int count = aGateWay.SelectAll().Count();
            while (count != 0)
            {
                HomePage item = new HomePage();
                item.getAttraction = aGateWay.SelectById(count);
                allList.Add(item);
                count = count - 1;
            }

            if (String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "Asc";
            }
            else if (sortOrder == "Asc")
            {
                sortOrder = "Desc";
            }
            else if (sortOrder == "Desc")
            {
                sortOrder = "Asc";
            }
            ViewBag.NameSort = sortOrder;
            var names = from c in allList
                             select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                names = names.Where(s => s.getAttraction.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Desc":
                    names = names.OrderByDescending(c => c.getAttraction.Name);
                    break;
                default:
                    names = names.OrderBy(c => c.getAttraction.Name);
                    break;
            }

            //For CategoryFilter
            ViewBag.Category = aQueryGateWay.getCategories();

            if ((searchString == null || searchString == ""))
            {
                var catModel = aQueryGateWay.getAttractionsByCat(category);

                List<HomePage> searchList = new List<HomePage>();
                foreach (Attractions item in catModel)
                {
                    HomePage chgItem = new HomePage();
                    chgItem.getAttraction = item;
                    searchList.Add(chgItem);
                }
                return View(searchList);
            }

            return View(names.ToList());
        }

        // POST: Attraction/ViewAttraction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewAttraction(int id)
        {
            Reviews review = new Reviews();
            Attractions_has_Reviews attractionReview = new Attractions_has_Reviews();
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
            rDGateway.Insert(review);


            attractionReview.Rid = review.Rid;
            attractionReview.Aid = id;
            raDGateway.Insert(attractionReview);
            return RedirectToAction("Attractions", id);
        }
    }
}