using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Tasks.Domain.AutoMapper
{
    public class RequestToDomainMapperTask : Profile
    {
        public RequestToDomainMapperTask()
        {
            CreateMap<TaskRequest, TaskEntity>();
        }
    }
}
