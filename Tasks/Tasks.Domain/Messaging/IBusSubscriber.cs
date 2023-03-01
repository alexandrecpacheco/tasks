using MediatR;

namespace Tasks.Domain.Messaging
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeEvent<TEvent>() where TEvent : IEvent, IRequest;
    }
}
