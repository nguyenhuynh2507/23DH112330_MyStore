﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _23DH112330_MyStore.Models.ViewModel
{
    public class ProductSearchVM
    {
        public string Username { get; set; }
        public string SearchTerm { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortOrder { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;

        public PagedList.IPagedList<Product>Products { get; set; }
    }
}