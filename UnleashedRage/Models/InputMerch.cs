using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnleashedRage.Models
{
    public class InputMerch
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3), MaxLength(32)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [DataType(DataType.Upload)]
        public IFormFile MerchImage { get; set; }
    }
}
