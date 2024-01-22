using System.ComponentModel.DataAnnotations;
using DAL.Exceptions;

namespace BLL.Services;

public static class ValidationHelper
{
    public static void ValidateObject(object obj)
    {
        ValidationContext validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
        List<ValidationResult> validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true))
        {
            IEnumerable<string> errors = validationResults.Select(r => r.ErrorMessage);
            throw new InputValidationException(string.Join(", ", errors));
        }
    }
}