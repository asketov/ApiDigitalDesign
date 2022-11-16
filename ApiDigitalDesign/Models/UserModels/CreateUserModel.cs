using System.ComponentModel.DataAnnotations;
using ApiDigitalDesign.AutoMapper;
using AutoMapper;
using Common.Helpers;
using DAL.Entities;

namespace ApiDigitalDesign.Models.UserModels
{
    public class CreateUserModel
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

       
    }
}
