using System.ComponentModel.DataAnnotations;
using Common.Helpers;
using DAL.Entities;

namespace BLL.ModelsDTO.UserModels
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(250)]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        [MinLength(6)]
        public string Password { get; set; }
        [MaxLength(250)]
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public DateTimeOffset BirthDate { get; set; }

        public User DtoToUser()
        {
            return new User()
            {
                Name = Name, Email = Email,
                PasswordHash = HashHelper.GetHash(Password),
                BirthDate = BirthDate
            };
        }
    }
}
