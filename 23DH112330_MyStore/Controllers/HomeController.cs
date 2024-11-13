using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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

            model.SkinCare = product.Where(p => p.Category.CategoryID == 11).Take(4).ToList();

            model.HairWash = product.Where(p => p.Category.CategoryID == 13).Take(4).ToList();

            model.FeaturedProducts = product.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

            model.NewProducts = product.OrderByDescending(p => p.OrderDetails.Count()).Take(20).ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public ActionResult ProductList(string searchTerm,string sortOrder, int? page)
        {

            var model = new HomeProductVM();
            var product = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                product = product.Where(p => p.ProductName.Contains(searchTerm) ||
                                  p.ProductDescription.Contains(searchTerm) ||
                                  p.Category.CategoryName.Contains(searchTerm));
            }

            int pageNumber = page ?? 1;
            int pageSize = 6;


            model.NewProducts = product.OrderBy(p => p.ProductName).ToPagedList(pageNumber, pageSize);

            switch (sortOrder)
            {
                case "daugoi": model.NewProducts = product.Where((p => p.Category.CategoryID == 13)).OrderBy(p => p.ProductName).ToPagedList(pageNumber, pageSize);  break;
                case "chamsocda": model.NewProducts = product.Where((p => p.Category.CategoryID == 11)).OrderBy(p => p.ProductName).ToPagedList(pageNumber, pageSize); break;
                case "giaasc": model.NewProducts = product.OrderBy(p => p.ProductPrice).ToPagedList(pageNumber, pageSize); break;
                case "giadesc": model.NewProducts = product.OrderByDescending(p => p.ProductPrice).ToPagedList(pageNumber, pageSize); break;
                case "duoi5tr": model.NewProducts = product.Where(p => p.ProductPrice < 5000000).OrderBy(p => p.ProductName).ToPagedList(pageNumber, pageSize); break;
                case "tu5->8": model.NewProducts = product.Where(p => p.ProductPrice >= 5000000 && p.ProductPrice <= 8000000 ).OrderBy(p => p.ProductName).ToPagedList(pageNumber, pageSize); break;
                case "tren8": model.NewProducts = product.Where(p => p.ProductPrice > 8000000).OrderBy(p => p.ProductName).ToPagedList(pageNumber, pageSize); break;
                default: model.NewProducts = product.OrderBy(p => p.ProductName).ToPagedList(pageNumber, pageSize); break;
            }
            model.SkinCare = product.Where(p => p.Category.CategoryID == 11).Take(4).ToList();

            model.HairWash = product.Where(p => p.Category.CategoryID == 13).Take(4).ToList();

            model.FeaturedProducts = product.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();


            model.SortOrder = sortOrder;

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