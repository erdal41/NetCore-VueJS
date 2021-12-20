using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Dtos
{
    public class UserDto : DtoBase
    {
        public User User { get; set; }
    }
}