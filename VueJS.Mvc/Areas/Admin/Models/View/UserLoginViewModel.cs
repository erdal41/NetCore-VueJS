using System.Collections.Generic;
using VueJS.Entities.Concrete;

namespace VueJS.Mvc.Areas.Admin.Models.View
{
    public class UserLoginViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ProfileImageId { get; set; }

        public IList<string> Roles { get; set; }
        public Upload ProfileImage { get; set; }
    }
}