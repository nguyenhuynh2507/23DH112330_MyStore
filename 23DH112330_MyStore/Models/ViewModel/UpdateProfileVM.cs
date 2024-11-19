using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _23DH112330_MyStore.Models.ViewModel
{
    public class UpdateProfileVM
    {
        [Required]
        public string CustomerName { get; set; }

        [Required, EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public string CustomerAddress { get; set; }
    }
}