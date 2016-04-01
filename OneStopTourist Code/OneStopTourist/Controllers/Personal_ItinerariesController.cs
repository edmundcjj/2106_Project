using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneStopTourist.Models;
using OneStopTourist.DAL;

namespace OneStopTourist.Controllers
{
    public class Personal_ItinerariesController : Controller
    {
        private Personal_ItinerariesGateway piGateway = new Personal_ItinerariesGateway();
        private ItineraryGateway iGateway = new ItineraryGateway();

        // GET: Personal_Itineraries/Save
        public ActionResult Save()
        {
            List<HomePage> allList = new List<HomePage>();

            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];

            if (sessionItinerary != null)
            {
                foreach (HomePage item in sessionItinerary)
                {
                    allList.Add(item);
                }
            }

            return View(allList);
        }

        // POST: Personal_Itineraries/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(HomePage Personal_Itineraries)
        {
            if (ModelState.IsValid)
            {
                //retrieve first two letters of the Nickname
                string retrievedLetter = Personal_Itineraries.getPersonalItinerary.Nickname.Substring(0, 2);

                //retrieve highest PIid in Personal_Itineraries table
                int value = piGateway.SelectHighestPIid();

                //calculate integer value for pin
                int result = value + 1;

                //store pin value
                Personal_Itineraries.getPersonalItinerary.Pin = retrievedLetter + result;

                //get content from session
                string content = "";
                List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];

                if (sessionItinerary != null)
                {
                    //for each item in the session, get its id and keep appending to string
                    foreach (HomePage item in sessionItinerary)
                    {
                        if (item.getAttraction != null) {
                            content += "A-" + item.getAttraction.Aid.ToString() + ",";
                        }
                        else
                        {
                            content += "S-" + item.getService.Sid.ToString() + ",";
                        }
                    }
                }

                Personal_Itineraries.getPersonalItinerary.Content = content;

                //insert content, nickname, pin into database
                piGateway.Insert(Personal_Itineraries.getPersonalItinerary);

                /*
                TempData["message"] = "Successfully saved. Your pin number is: " + Personal_Itineraries.Pin + ".Please sign in using your nickname and pin to retrieve your itinerary.";
                */

                //redirect back to "save" page
                //return RedirectToAction("Save");
            }
            //return RedirectToAction("Save");

            Session["myItinerary"] = null;

            List<HomePage> itineraryView = new List<HomePage>();
            HomePage personalItinerary = new HomePage();
            personalItinerary.getPersonalItinerary = Personal_Itineraries.getPersonalItinerary;
            itineraryView.Add(personalItinerary);

            return View(itineraryView);
        }

        //GET: Personal_Itineraries/Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: Personal_Itineraries/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "Content,Nickname")] Personal_Itineraries Personal_Itineraries, Itineraries Itineraries)
        {
            if (ModelState.IsValid)
            {
                //check and retrieve PIid if it is stored in Personal_Itinerary
                int value = piGateway.getItinerariesPId(Personal_Itineraries.Nickname, Personal_Itineraries.Content);

                //row exists
                if (value != -1)
                {
                    //delete row in Personal_Itineraries
                    piGateway.Delete(value);
                }

                //set value for Itineraries
                Itineraries.Nickname = Personal_Itineraries.Nickname;
                Itineraries.Content = Personal_Itineraries.Content;

                //insert into Itineraries
                iGateway.Insert(Itineraries);

                //redirect to "index" page of Itineraries
               //return Redirect("/Itinerary/Index");
            }

            /*NEED TO AMEND THIS*/
            List<HomePage> itineraryView = new List<HomePage>();
            HomePage personalItinerary = new HomePage();
            personalItinerary.getPersonalItinerary = Personal_Itineraries;
            itineraryView.Add(personalItinerary);

            return View(itineraryView);
        }

        //GET: Personal_Itineraries/SignIn
        public ActionResult SignIn()
        {
            return View();
        }

        // POST: Personal_Itineraries/SignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "Nickname,Pin")] Personal_Itineraries Personal_Itineraries)
        {
            //redirect back to "SignIn" page
            Personal_Itineraries loginRecord = piGateway.checkLogin(Personal_Itineraries.Nickname).First();
            if (loginRecord.Equals(""))
            {
                ViewBag.Identity = null;
                return RedirectToAction("SignIn");
            }
            else if (loginRecord.Pin == Personal_Itineraries.Pin)
            {
                ViewBag.Identity = loginRecord;
                return RedirectToAction("../Home/Index");
            }
            else
            {
                ViewBag.Identity = null;
                return RedirectToAction("SignIn");
            }
        }
       
    }
}
