namespace Application.Shared.Messaging;

// i prefer  to do like mediator even if  the command not return anything at least should return acknowledgment
public readonly struct Unit
{
    public static readonly Unit Value = new();
}

public interface IBaseCommand { };
public interface ICommand<out TResponse> : IBaseCommand { }
public interface ICommand : ICommand<Unit> { }