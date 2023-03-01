using MediatR;
using Tasks.Domain.Messaging;

namespace Tasks.Domain.Events
{
    public class BaseCompletedEvent : IEvent, IRequest
    {
        protected Guid _operationId;
        protected string? _origin;
        protected DateTime _date;

        public Guid OperationId
        {
            get
            {
                if (_operationId == Guid.Empty)
                {
                    _operationId = Guid.NewGuid();
                }
                return _operationId;
            }
            set => _operationId = Guid.NewGuid();
        }

        public string Origem
        {
            get
            {
                if (string.IsNullOrEmpty(_origin))
                {
                    _origin = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
                }
                return _origin;
            }
            set => _origin = value;
        }

        public DateTime Date
        {
            get
            {
                if (_date == DateTime.MinValue)
                {
                    _date = DateTime.Now;
                }
                return _date;
            }
            set => _date = value;
        }

    }
}
