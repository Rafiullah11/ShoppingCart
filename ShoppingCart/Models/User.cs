using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class User
    {
        [Required, MinLength(5, ErrorMessage = "Minimum length is 5")]
        [Display(Name ="Username")]
        public string UserName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string Password { get; set; }

    }
}
