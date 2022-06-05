using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
}
