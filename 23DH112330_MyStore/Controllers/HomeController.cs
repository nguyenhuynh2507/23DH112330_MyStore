using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _23DH112330_MyStore.Models;
using _23DH112330_MyStore.Models.ViewModel;
using PagedList;

namespace _23DH112330_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var product = db.Products.AsQueryable();

            if(!string.IsNullOrEmpty(searchTerm) )
            {
                model.SearchTerm = searchTerm;
                product = product.Where(p => p.ProductName.Contains(searchTerm) ||
                                  p.ProductDescription.Contains(searchTerm) ||
                                  p.Category.CategoryName.Contains(searchTerm));
            }
            
            int pageNumber = page ?? 1;
            int pageSize = 6;

            model.FeaturedProducts = product.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

            model.NewProducts = product.OrderBy(p => p.OrderDetails.Count()).Take(20).ToPagedList(pageNumber, pageSize);

            return View(model);
        }


        public ActionResult ProductDetail(int? id, int? quantity, int? page)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = db.Products.Find(id);
            if (pro == null) 
            { 
                return HttpNotFound();            
            }

            var products = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID!= pro.ProductID).AsQueryable();

            ProductDetailVM model = new ProductDetailVM();

            int pageNumber = page ?? 1;
            int pageSize = model.PageSize;
            model.product = pro;
            model.RelatedProducts = products.OrderBy(p => p.ProductID).Take(8).ToPagedList(pageNumber, pageSize);
            model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(8).ToPagedList(pageNumber, pageSize);

            if(quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }

            return View(model);
        }
    }
}