using System.Collections.Generic;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models.View
{
    public class UserViewModel
    {
        public User User { get; set; }
        public UserDto UserDto { get; set; }
        public UserAddDto UserAddDto { get; set; }
        public UserUpdateDto UserUpdateDto { get; set; }
        public UserRolesViewModel UserRolesViewModel { get; set; }
    }
}