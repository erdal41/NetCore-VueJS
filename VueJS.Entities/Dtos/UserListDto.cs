using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class UserListDto : DtoBase
    {
        public IList<User> Users { get; set; }
    }
}