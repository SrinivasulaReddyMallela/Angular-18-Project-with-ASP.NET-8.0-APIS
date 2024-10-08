﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGYM.ViewModels
{
    public class PostSAMLResponseModel
    {
        [Required(ErrorMessage = "Enter PostSAMLResponse")]
        public string PostSAMLResponse { get; set; }

         
        public string UserName { get; set; }
    }
    public class LoginRequestViewModel
    {
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        public string Token { get; set; }
        public int Usertype { get; set; }
    }
    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

}
