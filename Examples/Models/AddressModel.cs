using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Models
{
    public class AddressModel
    {        
        public string Name { get; set; }
        [Required]
        [MaxLength(15)]
        public string Street { get; set; }
        public string City { get; set; }
    }
}
