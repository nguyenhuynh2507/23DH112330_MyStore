using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _23DH112330_MyStore.Models.ViewModel
{
    public class OrderDetailVM
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}