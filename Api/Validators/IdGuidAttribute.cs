// ...existing code...
using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Api.Validators
{
    public class IdGuidAttribute : ValidationAttribute
    {
        public IdGuidAttribute()
            : base("This field {0} is not a valid Guid.")
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // use [Required] si debe existir
            }

            if (Guid.TryParse(value.ToString(), out _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
// ...existing code...