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
            return View();
        }

        // POST: Personal_Itineraries/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "Content,Nickname")] Personal_Itineraries Personal_Itineraries)
        {
            if (ModelState.IsValid)
            {
                //retrieve first two letters of the Nickname
                string retrievedLetter = Personal_Itineraries.Nickname.Substring(0, 2);

                //retrieve highest PIid in Personal_Itineraries table
                int value = piGateway.SelectHighestPIid();

                //calculate integer value for pin
                int result = value + 1;

                //store pin value
                Personal_Itineraries.Pin = retrievedLetter + result;

                //insert content, nickname, pin into database
                piGateway.Insert(Personal_Itineraries);

                /*
                TempData["message"] = "Successfully saved. Your pin number is: " + Personal_Itineraries.Pin + ".Please sign in using your nickname and pin to retrieve your itinerary.";
                */



                //redirect back to "save" page
                //return RedirectToAction("Save");


            }
            //return RedirectToAction("Save");

            return View(Personal_Itineraries);
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

            return View(Personal_Itineraries);
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
