﻿using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class PasswordResetQueryViewModel
    {
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Url]
        public string RedirectUrl { get; set; }
    }
}