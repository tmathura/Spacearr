using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Multilarr.Common.Tests.Common
{
    public static class TestModel
    {
        public static bool Validate(object model, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }
    }
}