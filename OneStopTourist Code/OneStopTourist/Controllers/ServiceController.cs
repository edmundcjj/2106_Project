using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneStopTourist.Models;
using OneStopTourist.DAL;

namespace OneStopTourist.Controllers
{
    public class ServiceController : Controller
    {
        private ReviewGateway rGateWay = new ReviewGateway();
        private ServiceGateway sGateWay = new ServiceGateway();
        private DataGateway<Services_has_Reviews> srGateWay = new DataGateway<Services_has_Reviews>();

        public ActionResult ViewService(int? id)
        {
            var reviewModel = rGateWay.getServiceReview(id);

            HomePage viewItem = new HomePage();
            viewItem.getService = sGateWay.SelectById(id);

            if (!reviewModel.Any())
            {
                List<HomePage> servicePage = new List<HomePage>();
                servicePage.Add(viewItem);

                return View(servicePage);
            }
            else {
                //Create a list to store both the service details and all of its reviews
                List<HomePage> reviewList = new List<HomePage>();
                //First review is stored in the first element of reviewList with the service details
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

        public ActionResult Services(string searchString, string category)
        {
            //retrieve all services
            List<HomePage> allList = new List<HomePage>();
            int count = sGateWay.SelectAll().Count();
            while (count != 0)
            {
                HomePage item = new HomePage();
                item.getService = sGateWay.SelectById(count);
                allList.Add(item);
                count = count - 1;
            }

            //For Category Filter
            ViewBag.Category = sGateWay.getCategories();

            if ((searchString == null || searchString == ""))
            {
                var catModel = sGateWay.getServicesByCat(category);

                //System.Diagnostics.Debug.WriteLine("testtest123" + category);

                List<HomePage> catList = new List<HomePage>();
                foreach (Services item in catModel)
                {
                    HomePage catItem = new HomePage();
                    catItem.getService = item;
                    catList.Add(catItem);
                }
                return View(catList);
            }

            else if (searchString != null)
            {
                //For Search Filter
                if ((category == null || category == ""))
                {
                    var searchModel = sGateWay.getServicesBySearch(searchString);

                    List<HomePage> searchList = new List<HomePage>();
                    foreach (Services item in searchModel)
                    {
                        HomePage searchItem = new HomePage();
                        searchItem.getService = item;
                        searchList.Add(searchItem);
                    }
                    return View(searchList);
                }

                //For Category & Search Filter
                else
                {
                    var twoFilterModel = sGateWay.getServicesTwoFilter(category, searchString);

                    List<HomePage> twoFilterList = new List<HomePage>();
                    foreach (Services item in twoFilterModel)
                    {
                        HomePage twoFilterItem = new HomePage();
                        twoFilterItem.getService = item;
                        twoFilterList.Add(twoFilterItem);
                    }
                    return View(twoFilterList);
                }
            }


            //Sort services by names
            var sortModel = sGateWay.SelectAllSortByName();
            List<HomePage> sortList = new List<HomePage>();
            foreach (Services item in sortModel)
            {
                HomePage sortItem = new HomePage();
                sortItem.getService = item;
                sortList.Add(sortItem);
            }
            return View(sortList);
        }

        // POST: Attraction/ViewAttraction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewService(int id)
        {
            Reviews review = new Reviews();
            Services_has_Reviews serviceReview = new Services_has_Reviews();
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


            serviceReview.Rid = review.Rid;
            serviceReview.Sid = id;
            srGateWay.Insert(serviceReview);
            return RedirectToAction("ViewService/" + id, id);
        }
    }
}