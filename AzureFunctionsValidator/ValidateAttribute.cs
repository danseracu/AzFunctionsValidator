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
    /// Use this attribute to validate the body of every request to the function using default data annotation validators
    /// </summary>
    public class ValidateAttribute: FunctionInvocationFilterAttribute
    {
        Type type;
        string paramName;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">Type of the model to validate</param>
        /// <param name="resultParameterName">Parameter name in which validation results will be injected</param>
        public ValidateAttribute(Type type, string resultParameterName)
        {
            this.type = type;
            this.paramName = resultParameterName;
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
            

            var results = new List<ValidationResult>();
            var context = new ValidationContext(content);

            Validator.TryValidateObject(content, context, results, true);

            //Flatten composite validation results
            var compositeValidationResults = results.Where(r => r is CompositeValidationResult).ToArray();

            foreach (var result in compositeValidationResults)
            {
                results.Remove(result);

                foreach (var res in (result as CompositeValidationResult).Results)
                {
                    results.Add(res);
                }
            }   

            //Inject validation results in the specified param name
            (executingContext.Arguments[paramName] as List<ValidationResult>).AddRange(results);

            return base.OnExecutingAsync(executingContext, cancellationToken);
        }
    }
}
