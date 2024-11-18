using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _23DH112330_MyStore.Models.ViewModel
{
    public class MyOrderListVM
    {
        [Display(Name = "Danh sách đơn hàng")]
        public List<MyOrderVM> Orders { get; set; }
    }
}