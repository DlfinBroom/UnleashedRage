using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnleashedRage.Models
{
    public class InputComicPage
    {
        [Required(ErrorMessage = "Volume is required")]
        public byte Volume { get; set; }
        [Required(ErrorMessage = "Issue is required")]
        public int Issue { get; set; }

        [Required(ErrorMessage = "File is required")]
        [DataType(DataType.Upload)]

        public bool SendEmail { get; set; }
        public IFormFile Image { get; set; }

        public InputComicPage()
        {
            SendEmail = true;
        }
    }
}
