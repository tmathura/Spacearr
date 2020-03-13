using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Helpers
{
    public class ValidationHelper : IValidationHelper
    {
        private readonly Page _page;

        public ValidationHelper(Page page)
        {
            _page = page;
        }

        public bool IsFormValid(object model)
        {
            HideValidationFields(model, _page);

            var errors = new List<ValidationResult>();
            var context = new ValidationContext(model);
            var isValid = Validator.TryValidateObject(model, context, errors, true);

            if (!isValid)
            {
                ShowValidationFields(errors, model, _page);
            }

            return !errors.Any();
        }

        private static void HideValidationFields(object model, Element page, string validationLabelSuffix = "Error")
        {
            if (model == null)
            {
                return;
            }
            var properties = GetValidatablePropertyNames(model);

            foreach (var propertyName in properties)
            {
                var errorControlName = $"{propertyName.Replace(".", "_")}{validationLabelSuffix}";
                var control = page.FindByName<Label>(errorControlName);

                if (control != null)
                {
                    control.IsVisible = false;
                }
            }
        }

        private static void ShowValidationFields(IEnumerable<ValidationResult> errors, object model, Element page, string validationLabelSuffix = "Error")
        {
            if (model == null)
            {
                return;
            }

            foreach (var error in errors)
            {
                var memberName = $"{model.GetType().Name}_{error.MemberNames.FirstOrDefault()}";

                memberName = memberName.Replace(".", "_");

                var errorControlName = $"{memberName}{validationLabelSuffix}";
                var control = page.FindByName<Label>(errorControlName);

                if (control != null)
                {
                    control.Text = $"{error.ErrorMessage}{Environment.NewLine}";
                    control.IsVisible = true;
                }
            }
        }

        private static IEnumerable<string> GetValidatablePropertyNames(object model)
        {
            var validatableProperties = new List<string>();
            var properties = GetValidatableProperties(model);

            foreach (var propertyInfo in properties)
            {
                var errorControlName = $"{propertyInfo.DeclaringType?.Name}.{propertyInfo.Name}";
                validatableProperties.Add(errorControlName);
            }

            return validatableProperties;
        }

        private static IEnumerable<PropertyInfo> GetValidatableProperties(object model)
        {
            var properties = model.GetType().GetProperties().Where(prop => prop.CanRead && prop.GetCustomAttributes(typeof(ValidationAttribute), true).Any() && prop.GetIndexParameters().Length == 0).ToList();
            return properties;
        }
    }
}
