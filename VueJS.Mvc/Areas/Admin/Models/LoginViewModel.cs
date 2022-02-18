using System;
using System.Collections.Generic;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class LoginViewModel
    {
        public List<Role> Roles { get; set; }
        public UserLoginDto UserLoginDto { get; set; }
        public UserLoginViewModel UserLoginViewModel { get; set; }
        public TokenModel TokenModel { get; set; }
        public GeneralSettingDto GeneralSettingDto { get; set; }
    }
}