using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionsValidator
{
    /// <summary>
    /// Interface to define custom validators
    /// </summary>
    public interface ICustomValidator
    {
        /// <summary>
        /// This method must be implemented in your validator class and must return a list of validation messages that will be later injected in your function
        /// </summary>
        /// <param name="content">The model to be validated</param>
        /// <returns>A list of validation errors</returns>
        List<ValidationResult> Validate(object content);
    }
}
