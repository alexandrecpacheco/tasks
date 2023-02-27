using Tasks.Domain.DTO.Response;
using Tasks.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Tasks.Domain.AutoMapper
{
    public class DomainToResponseMappingProfile : Profile
    {
        public DomainToResponseMappingProfile()
        {
            CreateMap<TaskEntity, TaskResponse>();
        }
    }
}
