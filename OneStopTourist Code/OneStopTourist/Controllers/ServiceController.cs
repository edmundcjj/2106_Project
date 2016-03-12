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

        public ActionResult ViewService(int? id)
        {
            var reviewModel = rGateWay.getServiceReview(id);

            List<HomePage> reviewList = new List<HomePage>();
            HomePage viewItem = new HomePage();
            viewItem.getService = sGateWay.SelectById(id);
            reviewList.Add(viewItem);
            foreach (Reviews item in reviewModel)
            {
                HomePage chgItem = new HomePage();
                chgItem.getServiceReview = item;
                reviewList.Add(chgItem);
            }

            return View(reviewList);
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
    }
}