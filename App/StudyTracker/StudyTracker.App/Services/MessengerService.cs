using CommunityToolkit.Mvvm.Messaging;

namespace StudyTracker.App.Services;

public class MessengerService : IMessengerService
{
    public MessengerService(IMessenger messenger)
    {
        Messenger = messenger;
    }

    public IMessenger Messenger { get; }

    public void Send<TMessage>(TMessage message)
        where TMessage : class
    {
        Messenger.Send(message);
    }
}