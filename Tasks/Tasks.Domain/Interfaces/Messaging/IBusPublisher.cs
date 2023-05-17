using Tasks.Domain.Events;

namespace Tasks.Domain.Interfaces.Messaging
{
    public interface IBusPublisher
    {
        public void SendProductMessage<T>(TaskCreatedEvent message);
    }
}
