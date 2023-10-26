using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Attributes {
    public class MinServiceTimeValidationAttribute : ValidationAttribute, IClientValidatable {
        private readonly string maxServiceTimePropertyName;

        public MinServiceTimeValidationAttribute(string maxServiceTimePropertyName) {
            this.maxServiceTimePropertyName = maxServiceTimePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            try {
                int maxServiceTime = (int) validationContext.ObjectType.GetProperty(maxServiceTimePropertyName).GetValue(validationContext.ObjectInstance, null);

                if ((int) value > maxServiceTime) {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return new ValidationResult("An error occurred while validating your request");
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context) {
            var rule = new ModelClientValidationRule {
                ErrorMessage = WebResources.MinServiceTimeValidationMessage,
                ValidationType = "minservicetimevalidation"
            };
            rule.ValidationParameters.Add("maxservicetime", maxServiceTimePropertyName);

            yield return rule;
        }
    }
}
