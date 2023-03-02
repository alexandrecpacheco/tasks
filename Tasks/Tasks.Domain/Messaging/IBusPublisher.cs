using Tasks.Domain.Events;

namespace Tasks.Domain.Messaging
{
    public interface IBusPublisher
    {
        public void SendProductMessage<T>(TaskCreatedEvent message);
    }
}
