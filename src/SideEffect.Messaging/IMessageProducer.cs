namespace SideEffect.Messaging;

/// <summary>
/// Represents message producer with support of Pub/Sub and RPC flows.
/// </summary>
public interface IMessageProducer : PubSub.IProducer, RPC.IProducer;
