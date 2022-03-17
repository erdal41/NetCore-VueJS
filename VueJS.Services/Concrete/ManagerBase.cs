using AutoMapper;
using VueJS.Data.Abstract;
using VueJS.Services.Helper.Abstract;

namespace VueJS.Services.Concrete
{
    public class ManagerBase
    {
        public ManagerBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        public ManagerBase(IUnitOfWork unitOfWork, IMapper mapper, IExtensionsHelper extensionsHelper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            ExtensionsHelper = extensionsHelper;
        }

        protected IUnitOfWork UnitOfWork { get; }
        protected IMapper Mapper { get; }
        protected IExtensionsHelper ExtensionsHelper { get; }
    }
}