using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Domain.Entities;

namespace Tasks.Infraestructure.Utils
{
    public interface IConfigurationTask
    {
        TaskRabbitMqConfiguration GetTaskConfiguration();
    }
}
