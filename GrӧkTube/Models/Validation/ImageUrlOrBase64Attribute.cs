using System.ComponentModel.DataAnnotations;

namespace GrӧkTube.Models.Validation
{
    public class ImageUrlOrBase64Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
                return ValidationResult.Success;

            var url = value.ToString();

            // Проверка на обычный URL
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var extension = Path.GetExtension(uri.AbsolutePath).ToLower();
                if (validExtensions.Contains(extension))
                    return ValidationResult.Success;
            }

            // Проверка на Base64
            if (url.StartsWith("data:image/") && url.Contains("base64,"))
                return ValidationResult.Success;

            return new ValidationResult("Допустимы только URL изображений (jpg, png, gif, bmp, webp) или Base64.");
        }
    }
}
