using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _23DH112330_MyStore.Models.ViewModel
{
    public class ChangePasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}