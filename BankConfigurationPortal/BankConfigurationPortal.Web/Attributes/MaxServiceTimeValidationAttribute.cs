using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Attributes {
    public class MaxServiceTimeValidationAttribute : ValidationAttribute, IClientValidatable {
        private readonly string minServiceTimePropertyName;

        public MaxServiceTimeValidationAttribute(string minServiceTimePropertyName) {
            try {
                this.minServiceTimePropertyName = minServiceTimePropertyName;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            try {
                int minServiceTime = (int) validationContext.ObjectType.GetProperty(minServiceTimePropertyName).GetValue(validationContext.ObjectInstance, null);

                if ((int) value < minServiceTime) {
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
            try {
                var rule = new ModelClientValidationRule {
                    ErrorMessage = WebResources.MaxServiceTimeValidationMessage,
                    ValidationType = "maxservicetimevalidation"
                };
                rule.ValidationParameters.Add("minservicetime", minServiceTimePropertyName);

                return new List<ModelClientValidationRule>() { rule };
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return default;
            }
        }
    }
}