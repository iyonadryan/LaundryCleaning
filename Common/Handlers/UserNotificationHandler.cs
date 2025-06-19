using LaundryCleaning.Common.Models.Messages;
using LaundryCleaning.Services.Attributes;
using LaundryCleaning.Services.Interfaces;

namespace LaundryCleaning.Common.Handlers
{
    [SystemMessageHandlerFor(nameof(UserNotification))]
    public class UserNotificationHandler : ISystemMessageHandler<UserNotification>
    {
        private readonly ILogger<UserNotificationHandler> _logger;

        public UserNotificationHandler(ILogger<UserNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(UserNotification message, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Notifikasi dari topik {nameof(UserNotification)}: {message.Message}");
            return Task.CompletedTask;
        }
    }
}
