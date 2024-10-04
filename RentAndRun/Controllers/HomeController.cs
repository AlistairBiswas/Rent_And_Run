using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rentandrun.Models;
using PagedList;

namespace Rentandrun.Controllers
{
    public class HomeController : Controller
    {
        private letdb01Entities db = new letdb01Entities();


        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;

            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;

            return View();
        }
        [HttpPost]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid && user.UserPassword.Equals(user.ConfirmPassword))
            {
                List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
                ViewBag.all_review = all_r;

                List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
                ViewBag.all_user = all_u;

                var us = db.Users.Where(u => u.UserEmail.Equals(user.UserEmail)).FirstOrDefault();
                if (us == null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    user.UserStatus = "Pending";
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = String.Format("Hello {0}. This email is already registered !" +
                   "Can not have two accounts by same email ! {1}", user.UserName, DateTime.Now.ToString());
                    return View();
                }

                //return Content("Hello ! Registration Successful");
                ViewBag.Message = String.Format("Hello {0}.Your account is registered successfully !" +
                "Now you can sign in ! {1}", user.UserName, DateTime.Now.ToString());
                return View();
            }
            //return Content("Password and Confirm Password Mismatched !");
            ViewBag.Message = String.Format("Hello {0}.Your Password and Confirm Password Mismatched !" +
                "Please Try Again ! {1}", user.UserName, DateTime.Now.ToString());
            return View();
        }
        [HttpPost]
        public ActionResult Login(TempUser tempUser)
        {
            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;

            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;

            if (ModelState.IsValid)
            {
                var user = db.Users.Where(u => u.UserName.Equals(tempUser.UserName)
                && u.UserPassword.Equals(tempUser.UserPassword)).FirstOrDefault();

                if (user != null)
                {
                    Session["user_email"] = user.UserEmail;
                    // return View("~/Views/AfterLogin/Index.cshtml");
                    return RedirectToAction("Index", "AfterLogin");
                }

            }
            // return Content("Failed Login !");
            ViewBag.Message = String.Format("Login Failed ! ");
            return View("Index");
        }


        [HttpPost]
        public ActionResult AdminLogin(tbl_admin ad)
        {
            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;

            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;

            if (ModelState.IsValid)
            {
                var user = db.tbl_admin.Where(u => u.ad_username.Equals(ad.ad_username)
                && u.ad_password.Equals(ad.ad_password)).FirstOrDefault();

                if (user != null)
                {
                    Session["ad_id"] = user.ad_id.ToString();
                    return RedirectToAction("Index", "Admin");
                }

            }
            // return Content("Failed Login !");
            ViewBag.Message = String.Format("Login Failed !");
            return View("Index");
        }

        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Booking()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Booking(Booking booking)
        {
            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;

            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;

            if (ModelState.IsValid && Session["user_email"]!=null)
            {
                string email = Convert.ToString(Session["user_email"]);
                var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();

                if(user.UserStatus != "Pending")
                {
                    booking.UserID = user.UserID;
                    booking.CarID = 1004;
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    booking.BookingStatus = "Pending";
                    booking.BookingDate = DateTime.Now.ToString("dd/MM/yyyy");
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();

                    Session["booking_id"] = db.Bookings.Max(item => item.BookingID).ToString();
                    return RedirectToAction("AllCarGridView");
                }
                ViewBag.Message = String.Format("Sorry ! Your booking can not take place as you are not an active" +
                "member. Please complete all of your profile details with valid information. ");
                return View("~/Views/AfterLogin/Index.cshtml");
            }
            ViewBag.Message = String.Format("Sorry! You can not book cars without logging in to your account .");
            return View("Index");
            //return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }

        public ActionResult AllCarGridView(int? page)
        {
            /*int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            // var list = db.Car_details.ToList();
            var list = db.Car_details.OrderByDescending(x => x.CarID).ToList();
            IPagedList<Car_details> stu = list.ToPagedList(pageindex, pagesize);

            return View(stu);*/
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var recentBooking = db.Bookings.OrderByDescending(b => b.BookingID).FirstOrDefault();
            DateTime recentBookingStartDate = DateTime.Now;
            if (recentBooking != null)
            {
                recentBookingStartDate = DateTime.ParseExact(recentBooking.Start_date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            var carDetails = db.Car_details.ToList();
            var availableCars = new List<Car_details>();

            foreach (var car in carDetails)
            {
                if (car.IsAvailable)
                {
                    availableCars.Add(car);
                }
                else
                {
                    bool isAvailable = car.Bookings.Any(booking =>
                        DateTime.ParseExact(booking.End_date, "MM/dd/yyyy", CultureInfo.InvariantCulture) <= recentBookingStartDate);

                    if (isAvailable)
                    {
                        availableCars.Add(car);
                    }
                }
            }
            
            IPagedList<Car_details> stu = availableCars.ToPagedList(pageindex, pagesize);

            return View(stu);
        }

        public ActionResult ViewCar(int? id)
        {
            List<Damage_details> all_d = db.Damage_details.ToList<Damage_details>();
            ViewBag.all_damage = all_d;

            Car_details c = db.Car_details.Where(x => x.CarID == id).SingleOrDefault();

            return View(c);
            //return View();
        }

    }
}