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

        public ActionResult ViewAttraction(int? id)
        {
            var reviewModel = rGateWay.getAttractionReview(id);

            List<HomePage> reviewList = new List<HomePage>();
            HomePage viewItem = new HomePage();
            viewItem.getAttraction = aGateWay.SelectById(id);
            reviewList.Add(viewItem);
            foreach (Reviews item in reviewModel)
            {
                HomePage chgItem = new HomePage();
                chgItem.getReview = item;
                reviewList.Add(chgItem);
            }

            return View(reviewList);
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
    }
}