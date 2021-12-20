using System.Collections.Generic;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class AdminMenuViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
        public GeneralSettingDto GeneralSettingDto { get; set; }
    }
}