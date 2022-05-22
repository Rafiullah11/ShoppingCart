using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ShoppingCart.Data
{
    public class FileExtensionAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var context = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var file = value as IFormFile;
            if (file!=null)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png" };
                bool result = extensions.Any(x => extension.EndsWith(x));
                if (!result)
                {
                    return new ValidationResult(getErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        private string getErrorMessage()
        {
            return "Allowed Extension are jpg and png";
        }
    }
}
