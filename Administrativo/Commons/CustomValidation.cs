using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string _extensions;

        public AllowedExtensionsAttribute(string extensions)
            : base("{0} não possui uma extensão de arquivo permitida.")
        {
            this._extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();

                string valueExtension = System.IO.Path.GetExtension(valueAsString);

                if (!this._extensions.Split('|').Contains(valueExtension))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            //string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule allowedExtensionsRules = new ModelClientValidationRule();
            allowedExtensionsRules.ErrorMessage = errorMessage;
            allowedExtensionsRules.ValidationType = "allowed-extensions"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            allowedExtensionsRules.ValidationParameters.Add("extensions", this._extensions);

            yield return allowedExtensionsRules;
        }

    }// End AllowedExtensions Class


}