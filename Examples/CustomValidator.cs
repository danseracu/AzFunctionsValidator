using AzureFunctionsValidator;
using Examples.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Examples
{
    public class CustomValidator : ICustomValidator
    {
        public List<ValidationResult> Validate(object content)
        {
            var user = content as UserModel;
            var errors = new List<ValidationResult>();

            if(!Regex.Match(user.Name, "^[a-zA-Z0-9]*$").Success)
            {
                errors.Add(new ValidationResult("Only alphanumeric characters are supported in name field"));
            }

            return errors;
        }
    }
}
