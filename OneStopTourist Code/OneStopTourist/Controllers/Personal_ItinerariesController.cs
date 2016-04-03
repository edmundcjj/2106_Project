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
        private AttractionGateway aGateway = new AttractionGateway();
        private ServiceGateway sGateway = new ServiceGateway();

        // GET: Personal_Itineraries/Save
        public ActionResult Save()
        {
            List<HomePage> allList = new List<HomePage>();
            if (Session["loggedInUser"] != null && Session["myItinerary"] == null)
            {
                //Session should be populated with DB gotten itinerary, else get it from database again
                //Getting the username from session
                string userName = Session["loggedInUser"].ToString();

                //Get personal itinerary from database
                var personalItinerary = piGateway.getPersonal_Itineraries(userName);
                Personal_Itineraries userItinerary = new Personal_Itineraries();
                userItinerary = personalItinerary.First();
                ViewBag.DBItinerary = userItinerary;

                //Getting each tourist spot from the content
                string itineraryString = userItinerary.Content;

                string[] itineraryList = itineraryString.Split(',');
                int getSpotsCount = itineraryList.Count();
                foreach (string spots in itineraryList.Take(getSpotsCount - 1))
                {
                    HomePage itinerarySpots = new HomePage();
                    string spotIDstring = spots.Substring(2, spots.Length - 2);
                    int spotID = Convert.ToInt32(spotIDstring);
                    if (spots.Substring(0, 2) == "A-")
                    {
                        Attractions attractionSpot = aGateway.SelectById(spotID);
                        itinerarySpots.getAttraction = attractionSpot;
                        allList.Add(itinerarySpots);
                    }
                    else if (spots.Substring(0, 2) == "S-")
                    {
                        Services serviceSpot = sGateway.SelectById(spotID);
                        itinerarySpots.getService = serviceSpot;
                        allList.Add(itinerarySpots);
                    }
                }
                Session["myItinerary"] = allList;

                return View(allList);
            }
            else
            {
                //Get personal itinerary from session
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
        }

        // POST: Personal_Itineraries/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(HomePage Personal_Itineraries)
        {
            List<HomePage> allList = new List<HomePage>();
            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];

            if (ModelState.IsValid)
            {
                // Check whether nickname exists in database
                if (piGateway.check_duplicate_nickname(Personal_Itineraries.getPersonalItinerary.Nickname) == false)
                {

                    // No duplicate nickname found in database
                    ViewBag.duplicateNick = "Available";

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

                    if (sessionItinerary != null)
                    {
                        //for each item in the session, get its id and keep appending to string
                        foreach (HomePage item in sessionItinerary)
                        {
                            if (item.getAttraction != null)
                            {
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


                    // Clear itinerary session after user has saved
                    Session["myItinerary"] = null;

                    List<HomePage> itineraryView = new List<HomePage>();
                    HomePage personalItinerary = new HomePage();
                    personalItinerary.getPersonalItinerary = Personal_Itineraries.getPersonalItinerary;
                    itineraryView.Add(personalItinerary);

                    return View(itineraryView);
                }
                else
                {
                    // duplicate nickname found in database
                    ViewBag.duplicateNick = "Nickname is taken, please try again.";

                    if (sessionItinerary != null)
                    {
                        foreach (HomePage item in sessionItinerary)
                        {
                            allList.Add(item);
                        }
                    }
                    return View(allList);
                }
            }

            //Return to original state, getting itinerary from session
            if (sessionItinerary != null)
            {
                foreach (HomePage item in sessionItinerary)
                {
                    allList.Add(item);
                }
            }
            return View(allList);
        }

        //GET: Personal_Itineraries/Upload
        public ActionResult Upload()
        {
            //Get personal itinerary from session
            Personal_Itineraries getViewBag = new Personal_Itineraries();
            getViewBag = (Personal_Itineraries) Session["Identity"];

            //check and retrieve PIid if it is stored in Personal_Itinerary
            int value = piGateway.getItinerariesPId(getViewBag.Nickname, getViewBag.Content);

            //row exists
            if (value != -1)
            {
                //delete row in Personal_Itineraries
                piGateway.Delete(value);
            }

            Itineraries thisItinerary = new Itineraries();
            thisItinerary.Nickname = getViewBag.Nickname;
            thisItinerary.Content = getViewBag.Content;
            
            iGateway.Insert(thisItinerary);
            TempData["returnShareStatus"] = "Your itinerary has been successfully shared!";
            Session["loggedInUser"] = null;
            Session["myItinerary"] = null;
            Session["Identity"] = null;

            //*******************************Maybe redirect to the uploaded Itinerary page?**************************************
            return RedirectToAction("../Home/Index");
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
            // Empty nickname, redirect back to "SignIn" page
            Personal_Itineraries loginRecord = piGateway.checkLogin(Personal_Itineraries.Nickname).First();
            if (loginRecord.Nickname.Equals(""))
            {
                Session["logInError"] = "Please enter valid log in credentials.";
                Session["Identity"] = null;
                Session["loggedInUser"] = null;
                return RedirectToAction("SignIn");
            }
            // Valid nickname, redirect back to index page with a slightly different layout
            else if (loginRecord.Pin == Personal_Itineraries.Pin)
            {
                ViewBag.Identity = loginRecord;
                Session["loggedInUser"] = loginRecord.Nickname;
                Session["Identity"] = loginRecord;
                Session["logInError"] = null;
                return RedirectToAction("../Home/Index");
            }
            // Invalid nickname, redirect back to "SignIn" page
            else
            {
                Session["logInError"] = "Incorrect nickname or PIN.";
                Session["loggedInUser"] = null;
                Session["Identity"] = null;
                return RedirectToAction("SignIn");
            }
        }

        public ActionResult LogOut()
        {
            ViewBag.Identity = null;
            Session["loggedInUser"] = null;
            Session["Identity"] = null;
            Session["logInError"] = null;
            Session["myItinerary"] = null;
            return RedirectToAction("../Home/Index");
        }
        
        public ActionResult Update(HomePage Personal_Itineraries)
        {
            List<HomePage> sessionItinerary = (List<HomePage>)Session["myItinerary"];
            if (sessionItinerary != null)
            {
                //get content from session
                string content = "";

                if (sessionItinerary != null)
                {
                    //for each item in the session, get its id and keep appending to string
                    foreach (HomePage item in sessionItinerary)
                    {
                        if (item.getAttraction != null)
                        {
                            content += "A-" + item.getAttraction.Aid.ToString() + ",";
                        }
                        else
                        {
                            content += "S-" + item.getService.Sid.ToString() + ",";
                        }
                    }
                }

                Personal_Itineraries getViewBag = new Personal_Itineraries();
                getViewBag = (Personal_Itineraries) Session["Identity"];
                getViewBag.Content = content;

                //insert content, nickname, pin into database
                piGateway.Update(getViewBag);
                TempData["returnUpdateStatus"] = "Your itinerary has been successfully updated!";
            }
            return RedirectToAction("../Personal_Itineraries/Save");
        }
    }
}
