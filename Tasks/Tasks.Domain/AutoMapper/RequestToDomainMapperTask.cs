using System.Diagnostics.CodeAnalysis;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Tasks.Domain.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class RequestToDomainMapperTask : Profile
    {
        public RequestToDomainMapperTask()
        {
            CreateMap<TaskRequest, TaskEntity>();
        }
    }
}
