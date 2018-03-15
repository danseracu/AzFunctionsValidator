using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionsValidator
{
    /// <summary>
    /// An attribute to prevent the Azure Functions runtime from trying to bind on the injected validation errors
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class IgnoreBindAttribute : Attribute
    {

    }

    public class IgnoreBindConfiguration : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<IgnoreBindAttribute>()
                .BindToInput(attr => new List<ValidationResult>());
        }
    }
}
