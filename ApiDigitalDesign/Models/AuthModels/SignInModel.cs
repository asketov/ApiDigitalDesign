﻿using System.ComponentModel.DataAnnotations;

namespace ApiDigitalDesign.Models.AuthModels
{
    public class SignInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
