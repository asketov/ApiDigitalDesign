using System.ComponentModel.DataAnnotations;

namespace ApiDigitalDesign.Models.AuthModels
{
    public class SignInModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
