using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnleashedRage.Models
{
    public class Merch
    {
        [Key]
        public int MerchID { get; set; }

        [MinLength(3), MaxLength(32)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public byte[] MerchImage { get; set; }
    }
}
