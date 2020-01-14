using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnleashedRage.Models
{
    public class InputComicPage
    {
        public byte Volume { get; set; }
        public int Issue { get; set; }
        
        public IFormFile Image { get; set; }
    }
}
