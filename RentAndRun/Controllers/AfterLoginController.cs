using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rentandrun.Models;

namespace trail3.Controllers
{
    public class AfterLoginController : Controller
    {
        private letdb01Entities db = new letdb01Entities();
        // GET: AfterLogin
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UserProfile()
        {
            string email = Convert.ToString(Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();

            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_booking = all_b;

            List<Car_details> all_c = db.Car_details.ToList<Car_details>();
            ViewBag.all_car = all_c;

            if (user != null)
            {
                Session["userId"] = user.UserID;
                ViewBag.u_id = user.UserID;
                return View(user);
            }
            return Content("NULL");
        }
        [HttpPost]
        public ActionResult UserProfile(User prouser)
        {
            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_booking = all_b;
            List<Car_details> all_c = db.Car_details.ToList<Car_details>();
            ViewBag.all_car = all_c;

            string email = Convert.ToString(Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();

            if (user != null)
            {
                user.UserAddress = prouser.UserAddress;
                user.UserContact_number = prouser.UserContact_number;
                user.UserDrivingID = prouser.UserDrivingID;
                user.UserPhoto = prouser.UserPhoto;
                user.UserNID = prouser.UserNID;
                user.UserDateOfBirth = Convert.ToString(prouser.UserDateOfBirth);
                user.UserStatus = "Pending";
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.u_id = user.UserID;
            ViewBag.Message = String.Format("Hello {0}.Your profile is updated successfully !" +
                 "{1}", user.UserName, DateTime.Now.ToString());
            return View(user);
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_booking = all_b;
            List<Car_details> all_c = db.Car_details.ToList<Car_details>();
            ViewBag.all_car = all_c;

            string email = Convert.ToString(Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();
            if (user != null)
            {
                return Content("" + user.UserName);
            }
            return Content("NULL");
        }
        [HttpPost]
        public ActionResult ChangePassword(User prouser)
        {
            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_booking = all_b;
            List<Car_details> all_c = db.Car_details.ToList<Car_details>();
            ViewBag.all_car = all_c;

            string email = Convert.ToString(Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();

            if (user != null)
            {

                if (prouser.UserPassword.Equals(user.UserPassword) && prouser.UserNewPassword1.Equals(prouser.UserNewPassword2))
                {
                    user.UserPassword = prouser.UserNewPassword1;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = String.Format("Hello {0}.Your profile is updated successfully !" +
                 "{1}", user.UserName, DateTime.Now.ToString());
                    return View(user);
                }
                else 
                {
                    ViewBag.Message = String.Format("Hello {0}.Your given password is incorrect !" +
                 "Cannot update your profile. Please Try Again ! {1}", user.UserName, DateTime.Now.ToString());
                    return View(user);
                }
                
            }
            return View(user);
        }
        public ActionResult AddReview(Review review)
        {
            string email = Convert.ToString(Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();
            if (ModelState.IsValid )
            {
                review.UserID = Convert.ToInt32(Session["userId"]);
                review.CarID = 1004;
                db.Reviews.Add(review);
                db.SaveChanges();
                review.Validation = "Not Valid"; 
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();

                List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
                ViewBag.all_booking = all_b;

                List<Car_details> all_c = db.Car_details.ToList<Car_details>();
                ViewBag.all_car = all_c;

                ViewBag.u_id = user.UserID;

                ViewBag.Message = String.Format("Thanks for your valuable" +
                    " review ! {0}",  DateTime.Now.ToString());
                return View("UserProfile",user);
            }
            return View("UserProfile");
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Abandon();

            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;

            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;

            return RedirectToAction("Index", "Home");
        }
        private List<User>GetUsers()
        {
            List<User> users = new List<User>();
            return users;
        }
        private List<Booking> GetBookings()
        {
            List<Booking> bookings = new List<Booking>();
            return bookings;
        }

        public ActionResult Payment(int? id)
        {
            string email = Convert.ToString(Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();

            int b_id = Convert.ToInt32(Session["booking_id"]);
            var booking = db.Bookings.Where(u => u.BookingID.Equals(b_id)).FirstOrDefault();

            Car_details c = db.Car_details.Where(x => x.CarID == id).SingleOrDefault();

            if (user != null && booking!= null)
            {
                booking.CarID = c.CarID;
                booking.BookingStatus = "Not verified";

                string sdate = booking.Start_date;
                string edate = booking.End_date;

                double days = (Convert.ToDateTime(edate) - Convert.ToDateTime(sdate)).TotalDays;
                double amount = Convert.ToDouble(c.Daily_Fee) * days;
                booking.TotalAmount = Convert.ToString(amount);

                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();

                Session["d"] = days.ToString();
                ViewData["Message"] = booking;
                ViewData["day"] = days;
                return View(user);
            }
            ViewBag.Message = String.Format("Sorry! Your are not Logged in or Incomplete booking form .");
            return View();
        }

        [HttpPost]
        public ActionResult AfterPayment(string TransectionID)
        {
            string t = TransectionID;
            string email = Convert.ToString(Session["user_email"]);
            var user = db.Users.Where(u => u.UserEmail.Equals(email)).FirstOrDefault();

            int b_id = Convert.ToInt32(Session["booking_id"]);
            var booking = db.Bookings.Where(u => u.BookingID.Equals(b_id)).FirstOrDefault();

            if (user != null )
            {
                booking.TransectionID = t;

                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = String.Format("Congratulations! Your booking is completed (PAID). Please check " +
                    "your profile for admin confirmation . ");
                return View("Index");
            }
            ViewBag.Message = String.Format("Sorry ! Your booking can not take place as you are not an active" +
                "member. Please complete all of your profile details with valid information. ");
            return View("Index");
        }



    }
}