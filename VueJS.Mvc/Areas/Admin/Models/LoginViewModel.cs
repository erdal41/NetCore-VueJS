using System;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public User User { get; set; }
        public UserLoginDto UserLoginDto { get; set; }
        public GeneralSettingDto GeneralSettingDto { get; set; }
        public TokenModel TokenModel { get; set; }   
    }
}