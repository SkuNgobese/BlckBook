using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace The_Book.Models
{
    public class RSAIDNumber : ValidationAttribute, IClientValidatable
    {
        public RSAIDNumber(): base("{0} is not a valid South African ID Number")
        {

        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IdentityInfo idInfo = new IdentityInfo(value.ToString());

            if (!idInfo.IsValid)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "rsaid";
            yield return rule;
        }
    }
}