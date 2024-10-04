using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Rentandrun.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;

namespace letdb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private letdb01Entities db = new letdb01Entities();
        [HttpGet]
        public ActionResult Index( )
        {
            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_book = all_b;

            List<Car_details> all_c = db.Car_details.OrderByDescending(x => x.CarID).ToList<Car_details>();
            ViewBag.all_cars = all_c;

            return View();
        }
        [HttpPost]
        public ActionResult Index(tbl_admin avm1)
        {
            tbl_admin ad = db.tbl_admin.Where(x => x.ad_username == avm1.ad_username && x.ad_password == avm1.ad_password).SingleOrDefault();
            if (ad != null)
            {
                Session["ad_id"] = ad.ad_id.ToString();
                return View("BookingDetails");
            }
            else
            {
                ViewBag.error = "Invalid username or password";
            }
            return View();
        }
        [HttpGet]
        public ActionResult BookingDetails()
        {
            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_booking = all_b;
            
            return View();
        }
        [HttpPost]
        public ActionResult BookingDetails(int BookingID)
        {
            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_booking = all_b;
            var book = db.Bookings.Where(b => b.BookingID.Equals(BookingID)).FirstOrDefault();
            if (book != null)
            {
                Session["booking_id"] = book.BookingID;
                return View(book);
            }
            return View();
            return View();
        }
        public ActionResult VerifyBooking()
        {
            List<Booking> all_b = db.Bookings.OrderByDescending(x => x.BookingID).ToList<Booking>();
            ViewBag.all_booking = all_b;
            int id = Convert.ToInt32(Session["booking_id"]);
            var book = db.Bookings.Where(b => b.BookingID.Equals(id)).FirstOrDefault();
            if (book != null)
            {
                // Update car availability flag to unavailable
                var bookedCar = db.Car_details.Find(book.CarID);
                if (bookedCar != null)
                {
                    bookedCar.IsAvailable = false;
                    db.Entry(bookedCar).State = EntityState.Modified;
                }

                book.BookingStatus = "Verified";
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                
                return View("BookingDetails",book);
            }
            return View("BookingDetails");
            //return View();
        }
        [HttpGet]
        public ActionResult MemberManagement()
        {
            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;
            return View();
        }
        [HttpPost]
        public ActionResult MemberManagement(User prouser)
        {
            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;
            var user = db.Users.Where(u => u.UserID.Equals(prouser.UserID)).FirstOrDefault();
            if (user != null)
            {
                Session["user_id"] = user.UserID;
                return View(user);
            }
            return View();
        }
        public ActionResult ActiveMember()
        {
            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;
            int id = Convert.ToInt32(Session["user_id"]);
            var user = db.Users.Where(u => u.UserID.Equals(id)).FirstOrDefault();
            if (user != null)
            {
                user.UserStatus = "Active";
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return View("MemberManagement", user);
            }
            return View("MemberManagement");
        }
        public ActionResult PendingMember()
        {
            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;
            int id = Convert.ToInt32(Session["user_id"]);
            var user = db.Users.Where(u => u.UserID.Equals(id)).FirstOrDefault();
            if (user != null)
            {
                user.UserStatus = "Pending";
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return View("MemberManagement", user);
            }
            return View("MemberManagement");
        }
        public ActionResult DeleteMember()
        {
            List<User> all_u = db.Users.OrderByDescending(x => x.UserID).ToList<User>();
            ViewBag.all_user = all_u;
            int id = Convert.ToInt32(Session["user_id"]);
            var user = db.Users.Where(u => u.UserID.Equals(id)).FirstOrDefault();
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return View("MemberManagement", user);
            }
            return View("MemberManagement");
        }

        public ActionResult Create()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(Car_details cvm, HttpPostedFileBase imgfile)
        {
            if (cvm == null)
            {
                throw new ArgumentNullException(nameof(cvm));
            }
            
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                Car_details cat = new Car_details();
                cat.CarNumber = cvm.CarNumber;
                cat.CarImage = path;
                cat.RegistrationYear = cvm.RegistrationYear;
                cat.EngineNumber = cvm.EngineNumber;
                cat.CarDetails = cvm.CarDetails;
                cat.Daily_Fee = cvm.Daily_Fee;
                db.Car_details.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Display_Car_details");
            }

            return View();
        }


        public ActionResult Display_Car_details(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            // var list = db.Car_details.ToList();
            var list = db.Car_details.OrderByDescending(x => x.CarID).ToList();
            IPagedList<Car_details> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }

        public ActionResult DeleteCar(int? id)
        {
            Car_details c = db.Car_details.Where(x => x.CarID == id).SingleOrDefault();
            db.Car_details.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Display_Car_details");
            // return View();
            // return View();
        }


        public ActionResult Damage_Control_upload(Damage_details cvm,
            HttpPostedFileBase imgfile_front, HttpPostedFileBase imgfile_back,
            HttpPostedFileBase imgfile_right, HttpPostedFileBase imgfile_left)
        {
            string pathfront = uploadimgfile(imgfile_front);
            string pathback = uploadimgfile(imgfile_back);
            string pathright = uploadimgfile(imgfile_right);
            string pathleft = uploadimgfile(imgfile_left);
            if (pathfront.Equals("-1") || pathback.Equals("-1")
                || pathright.Equals("-1") || pathleft.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                Damage_details cat = new Damage_details();
                cat.CarID = cvm.CarID;
                cat.CarImage_Front = pathfront;
                cat.CarImage_Back = pathback;
                cat.CarImage_Right = pathright;
                cat.CarImage_Left = pathleft;
                db.Damage_details.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
            return View();
        }


        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
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

        [HttpGet]
        public ActionResult Reviews()
        {
            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;
            return View();
        }
        [HttpPost]
        public ActionResult Reviews(Review review)
        {
            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;

            var rev = db.Reviews.Where(r => r.ReviewID.Equals(review.ReviewID)).FirstOrDefault();
            if (rev != null)
            {
                Session["review_id"] = rev.ReviewID;
                return View(rev);
            }
            return View();
            //return View();
        }
        public ActionResult VerifyComment()
        {
            List<Review> all_r = db.Reviews.OrderByDescending(x => x.ReviewID).ToList<Review>();
            ViewBag.all_review = all_r;
            int id = Convert.ToInt32(Session["review_id"]);
            var rev = db.Reviews.Where(b => b.ReviewID.Equals(id)).FirstOrDefault();
            if (rev != null)
            {
                rev.Validation = "Valid";
                db.Entry(rev).State = EntityState.Modified;
                db.SaveChanges();

                return View("Reviews", rev);
            }
            return View("Reviews");
            // return View();
        }
        public ActionResult Chat()
        {
            return View();
        }
    }
}