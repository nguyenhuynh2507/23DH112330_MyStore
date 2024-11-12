using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _23DH112330_MyStore.Models.ViewModel
{
    public class HomeProductVM
    {

        public Product product { get; set; }
        public int quantity { get; set; } = 1;
        public string Username { get; set; }
        public string SearchTerm { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string SortOrder { get; set; }

        public List<Product> FeaturedProducts { get; set; }
        public List<Product> SkinCare { get; set; }
        public List<Product> HairWash { get; set; }

        public PagedList.IPagedList<Product> NewProducts { get; set; }


    }
}