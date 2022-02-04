using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using System.Collections.Generic;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class UserRolesViewModel
    {
        public int UserId { get; set; }
        public List<Role> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
        public UserDto UserDto { get; set; }
    }
}