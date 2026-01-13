using SideEffect.Messaging;

namespace SideEffect.Demo.Common;

public class PubSubMessage(MessageModel data) : EventMessage<MessageModel>(data);
