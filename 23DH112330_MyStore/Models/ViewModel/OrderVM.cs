using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _23DH112330_MyStore.Models.ViewModel
{
    public class OrderVM
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderDetailVM> OrderDetails { get; set; }
    }
}