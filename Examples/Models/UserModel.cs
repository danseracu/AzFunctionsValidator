using AzureFunctionsValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Models
{
    public class UserModel
    {
        [Required]
        public string Name { get; set; }

        [Range(18, 55)]
        public int Age { get; set; }

        [Required]
        [ValidateObject]
        public AddressModel Address { get; set; }
    }
}
