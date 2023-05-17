using System.Diagnostics.CodeAnalysis;
using Tasks.Domain.DTO.Response;
using Tasks.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Tasks.Domain.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class DomainToResponseMappingProfile : Profile
    {
        public DomainToResponseMappingProfile()
        {
            CreateMap<TaskEntity, TaskResponse>();
        }
    }
}
