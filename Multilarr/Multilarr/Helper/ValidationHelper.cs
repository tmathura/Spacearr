using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Multilarr.Helper
{
    public static class ValidationHelper
    {
        public static bool IsFormValid(object model, Page page)
        {
            HideValidationFields(model, page);

            var errors = new List<ValidationResult>();
            var context = new ValidationContext(model);
            var isValid = Validator.TryValidateObject(model, context, errors, true);

            if (!isValid)
            {
                FormatErrors(errors);
                ShowValidationFields(errors, model, page);
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

        private static void FormatErrors(IEnumerable<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                var memberName = ((string[])error.MemberNames)[0];

                var pattern = $@"\b{memberName}\b";

                if (!string.IsNullOrWhiteSpace(memberName))
                {
                    var replaceText = new StringBuilder(memberName.Length * 2);
                    replaceText.Append(memberName[0]);

                    for (var i = 1; i < memberName.Length; i++)
                    {
                        if (char.IsUpper(memberName[i]) && memberName[i - 1] != ' ')
                            replaceText.Append(' ');
                        replaceText.Append(memberName[i]);
                    }

                    error.ErrorMessage = Regex.Replace(error.ErrorMessage, pattern, replaceText.ToString(), RegexOptions.IgnoreCase);
                }
            }
        }
    }
}
