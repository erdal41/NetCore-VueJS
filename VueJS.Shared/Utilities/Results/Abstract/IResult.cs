using VueJS.Shared.Entities.Concrete;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;

namespace VueJS.Shared.Utilities.Results.Abstract
{
    public interface IResult
    {
        public ResultStatus ResultStatus { get; }
        public string Message { get; }
        public Exception Exception { get; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}