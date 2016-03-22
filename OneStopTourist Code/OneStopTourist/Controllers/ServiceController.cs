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
        private DataGateway<Services> sGateWay = new DataGateway<Services>();
        private ReviewGateway rGateWay = new ReviewGateway();
        private ServiceGateway sQueryGateWay = new ServiceGateway();
        private DataGateway<Reviews> rDGateway = new DataGateway<Reviews>();
        private DataGateway<Services_has_Reviews> rsDGateway = new DataGateway<Services_has_Reviews>();

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

        public ActionResult Services(string sortOrder, string searchString, string category)
        {
            List<HomePage> allList = new List<HomePage>();
            int count = sGateWay.SelectAll().Count();
            while (count != 0)
            {
                HomePage item = new HomePage();
                item.getService = sGateWay.SelectById(count);
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
            ViewBag.CategorySort = sortOrder;
            var categories = from c in allList
                             select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(s => s.getService.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Desc":
                    categories = categories.OrderByDescending(c => c.getService.Category);
                    break;
                default:
                    categories = categories.OrderBy(c => c.getService.Category);
                    break;
            }

            //For CategoryFilter
            ViewBag.Category = sQueryGateWay.getCategories();

            if ((searchString == null || searchString == ""))
            {
                var catModel = sQueryGateWay.getServicesByCat(category);

                List<HomePage> searchList = new List<HomePage>();
                foreach (Services item in catModel)
                {
                    HomePage chgItem = new HomePage();
                    chgItem.getService = item;
                    searchList.Add(chgItem);
                }
                return View(searchList);
            }

            return View(categories.ToList());
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
            rDGateway.Insert(review);


            serviceReview.Rid = review.Rid;
            serviceReview.Sid = id;
            rsDGateway.Insert(serviceReview);
            return RedirectToAction("Services", id);
        }
    }
}