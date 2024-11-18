using _23DH112330_MyStore.Models;
using _23DH112330_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _23DH112330_MyStore.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    UserRole = "C"
                };
                db.Users.Add(user);
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username 
                && u.Password == model.Password);
                if (user != null)
                {
                    if (user.UserRole == "C") 
                    {
                        Session["Username"] = user.Username;
                        Session["UserRole"] = user.UserRole;
                        FormsAuthentication.SetAuthCookie(user.Username, false);

                        Debug.WriteLine("User.Identity.IsAuthenticated: " + User.Identity.IsAuthenticated);
                        Debug.WriteLine("User.Identity.Name: " + User.Identity.Name);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.UserRole == "A")
                    {
                        Session["Username"] = user.Username;
                        Session["UserRole"] = user.UserRole;
                        FormsAuthentication.SetAuthCookie(user.Username, false);

                        Debug.WriteLine("User.Identity.IsAuthenticated: " + User.Identity.IsAuthenticated);
                        Debug.WriteLine("User.Identity.Name: " + User.Identity.Name);
                        return RedirectToAction("Index", "Admin/HomeAdmin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "tên đăng nhập hoặc mật khẩu không đúng");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult ProfileInfo()
        {
            return View();
        }
        public ActionResult UpdateProfile()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult VerifyState()
        {
            if (Session["UserRole"] != null)
            {
                if (Session["UserRole"].ToString() == "A")
                {
                    ViewBag.Message = "Your application description page for Admin.";
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else if (Session["UserRole"].ToString() == "C")
                {
                    return View();
                }
            }

            ViewBag.Message = "Bạn không được phép truy cập";
            return RedirectToAction("Login", "Account");
        }
    }
}