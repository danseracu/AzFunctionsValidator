using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionsValidator
{
    /// <summary>
    /// Use this attribute to validate to model on the body of all incoming requests to this function using a custom validator
    /// </summary>
    public class CustomValidateAttribute : FunctionInvocationFilterAttribute
    {

        Type type;
        string paramName;
        ICustomValidator customValidator;

        /// <summary>
        /// Use this attribute to validate to model on the body of all incoming requests to this function using a custom validator
        /// </summary>
        /// <param name="type">The type of the model to be validated</param>
        /// <param name="resultParameterName">Paramer name to inject validation results. Must be of type <code>ICollection<ValidationResult></code>/param>
        /// <param name="customValidatorType">Type of the custom validator to use</param>
        public CustomValidateAttribute(Type type, string resultParameterName, Type customValidatorType)
        {
            this.type = type;
            this.paramName = resultParameterName;
            this.customValidator = Activator.CreateInstance(customValidatorType) as ICustomValidator;
        }

        /// <summary>
        /// Use this attribute to validate to model on the body of all incoming requests to this function using a custom validator
        /// </summary>
        /// <param name="type">The type of the model to be validated</param>
        /// <param name="resultParameterName">Paramer name to inject validation results. Must be of type <code>ICollection<ValidationResult></code></param>
        /// <param name="customValidatorFactory">A method that returns an instance of the custom validator to use</param>
        public CustomValidateAttribute(Type type, string resultParameterName, Func<ICustomValidator> customValidatorFactory)
        {
            this.type = type;
            this.paramName = resultParameterName;
            this.customValidator = customValidatorFactory();
        }

        public override Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var req = executingContext.Arguments.FirstOrDefault().Value as HttpRequestMessage;
            object content = null;

            try
            {
                //Attempt to deserialize the body into the type set on the attribute
                content = JsonConvert.DeserializeObject(req.Content.ReadAsStringAsync().Result, type);
            }
            catch (Exception)
            {
                (executingContext.Arguments[paramName] as List<ValidationResult>).Add(new ValidationResult("Error parsing request body"));
                return base.OnExecutingAsync(executingContext, cancellationToken);
            }

            //Validate the parsed model and inject the data in the specified parameter
            var results = customValidator.Validate(content);
            (executingContext.Arguments[paramName] as List<ValidationResult>).AddRange(results);

            return base.OnExecutingAsync(executingContext, cancellationToken);
        }

    }
}
