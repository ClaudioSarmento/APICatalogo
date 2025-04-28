using System.ComponentModel.DataAnnotations;

namespace APICatalago.Validations
{
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var texto = value?.ToString()?.TrimStart();

            if (string.IsNullOrEmpty(texto) || char.IsUpper(texto[0]))
                return ValidationResult.Success;

            return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula.");
        }
    }
}
