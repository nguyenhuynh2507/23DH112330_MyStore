using _23DH112330_MyStore.Models;
using _23DH112330_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23DH112330_MyStore.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Checkout()
        {
            var cart = (Cart)Session["Cart"];
            Debug.WriteLine(cart == null ? "Session['Cart'] is null" : "Session['Cart'] loaded with item count: " + cart.Items.Count());

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            Debug.WriteLine("User.Identity.IsAuthenticated: " + User.Identity.IsAuthenticated);
            Debug.WriteLine("User.Identity.Name: " + User.Identity.Name);
            if (user == null) { return RedirectToAction("Login", "Account"); }


            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null) { return RedirectToAction("Login", "Account"); }

            var model = new CheckoutVM
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.Items.Sum(item => item.TotalPrice),
                OrderDate = DateTime.Now,
                ShippingAddress = customer.CustomerAddress,
                CustomerID = customer.CustomerID,
                Username = customer.Username
            };
            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {

                var cart = (Cart)Session["Cart"];
                if (cart == null || !cart.Items.Any())
                {
                    return RedirectToAction("Index", "Home");
                }
                var user = db.Users.SingleOrDefault(u => u.Username ==  User.Identity.Name);
                if (user == null) { return RedirectToAction("Login", "Account"); }


                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if (customer == null) { return RedirectToAction("Login", "Account"); }

                if(model.PaymentMethod == "Paypal") { return RedirectToAction("PaymentWithPaypal", "Paypal", model); }

                string paymentStatus = "Chưa thanh toán";
                switch(model.PaymentStatus)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán bằng tiền mặt"; break;
                    case "Paypal": paymentStatus = "Thanh toán bằng Paypal"; break;
                    case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
                    default: paymentStatus = "Chưa thanh toán"; break; 
                }
                var order = new Order()
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cart.Items.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                };
                db.Orders.Add(order);
                db.SaveChanges();
                Session["Cart"] = null;
                return RedirectToAction("OrderSuccess", new {id = order.OrderID});
            }
            return View(model);
        }
    }
}