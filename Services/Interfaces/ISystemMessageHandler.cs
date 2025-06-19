namespace LaundryCleaning.Services.Interfaces
{
    public interface ISystemMessageHandler<in T>
    {
        Task HandleAsync(T message, CancellationToken cancellationToken);
    }
}
