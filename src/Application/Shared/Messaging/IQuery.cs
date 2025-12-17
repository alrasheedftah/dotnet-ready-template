namespace Application.Shared.Messaging;

public interface IBaseQuery { }

public interface IQuery<out TResponse> : IBaseQuery { }
