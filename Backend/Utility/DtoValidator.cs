using System.ComponentModel.DataAnnotations;

namespace Backend.Utility;

public class DtoValidator
{
    public static bool ValidateObject<T>(T dto, out List<ValidationResult> messages) where T : IValidatableObject
    {
        var context = new ValidationContext(dto, null, null);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, context, results, true);
        messages = results;
        return isValid;
    }
}