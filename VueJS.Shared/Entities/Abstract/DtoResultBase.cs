using VueJS.Shared.Utilities.Results.ComplexTypes;

namespace VueJS.Shared.Entities.Abstract
{
    public abstract class DtoResultBase
    {
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}