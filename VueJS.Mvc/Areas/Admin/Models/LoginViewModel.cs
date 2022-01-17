using System;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public UserLoginDto UserLoginDto { get; set; }
        public GeneralSettingDto GeneralSettingDto { get; set; }

        public string Token { get; set; }   
        public DateTime Expiration { get; set; }   
    }
}