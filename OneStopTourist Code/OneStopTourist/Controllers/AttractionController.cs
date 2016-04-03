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
        private ReviewGateway rGateWay = new ReviewGateway();
        private AttractionGateway aGateWay = new AttractionGateway();
        private DataGateway<Attractions_has_Reviews> arGateWay = new DataGateway<Attractions_has_Reviews>();

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

        public ActionResult Attractions(string searchString, string category)
        {
            //retrieve all attraction
            List<HomePage> allList = new List<HomePage>();
            int count = aGateWay.SelectAll().Count();
            while (count != 0)
            {
                HomePage item = new HomePage();
                item.getAttraction = aGateWay.SelectById(count);
                allList.Add(item);
                count = count - 1;
            }

            //For Category Filter
            ViewBag.Category = aGateWay.getCategories();

            if ((searchString == null || searchString == ""))
            {
                var catModel = aGateWay.getAttractionsByCat(category);

                //System.Diagnostics.Debug.WriteLine("testtest123" + category);

                List<HomePage> catList = new List<HomePage>();
                foreach (Attractions item in catModel)
                {
                    HomePage catItem = new HomePage();
                    catItem.getAttraction = item;
                    catList.Add(catItem);
                }
                return View(catList);
            }

            else if (searchString != null)
            {
                //For Search Filter
                if ((category == null || category == ""))
                {
                    var searchModel = aGateWay.getAttractionsBySearch(searchString);

                    List<HomePage> searchList = new List<HomePage>();
                    foreach (Attractions item in searchModel)
                    {
                        HomePage searchItem = new HomePage();
                        searchItem.getAttraction = item;
                        searchList.Add(searchItem);
                    }
                    return View(searchList);
                }

                //For Category & Search Filter
                else
                {
                    var twoFilterModel = aGateWay.getAttractionTwoFilter(category, searchString);

                    List<HomePage> twoFilterList = new List<HomePage>();
                    foreach (Attractions item in twoFilterModel)
                    {
                        HomePage twoFilterItem = new HomePage();
                        twoFilterItem.getAttraction = item;
                        twoFilterList.Add(twoFilterItem);
                    }
                    return View(twoFilterList);
                }
            }


            //Sort attraction by names
            var sortModel = aGateWay.SelectAllSortByName();
            List<HomePage> sortList = new List<HomePage>();
            foreach (Attractions item in sortModel)
            {
                HomePage sortItem = new HomePage();
                sortItem.getAttraction = item;
                sortList.Add(sortItem);
            }
            return View(sortList);
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
            rGateWay.Insert(review);


            attractionReview.Rid = review.Rid;
            attractionReview.Aid = id;
            arGateWay.Insert(attractionReview);
            return RedirectToAction("ViewAttraction/"+id, id);
        }
    }
}