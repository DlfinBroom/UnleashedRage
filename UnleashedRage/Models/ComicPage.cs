using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnleashedRage.Models
{
    public class ComicPage
    {
        [Key]
        public int PageID { get; set; }
        public byte Volume { get; set; }
        public int Issue { get; set; }

        public byte[] Image { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
}
