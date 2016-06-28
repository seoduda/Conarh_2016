using System;
using Conarh_2016.Core.Commands;
using Conarh_2016.Core;

namespace Conarh_2016.Application.Commands.Local
{
    public sealed class PushNotificationParameters : LocalCommandParameters
    {
        public readonly string NotificationName;
        public readonly object NotificationData;

        public PushNotificationParameters(ICommandHandler handler, string notificationName, 
            object data = null, Action<LocalCommand> onSuccess = null,  Action<LocalCommand> onFault = null)
            : base(handler, onSuccess, onFault)
        {
            NotificationName = notificationName;
            NotificationData = data;
        }
    }

    public sealed class PushNotificationCommand : LocalCommand
    {
        protected override void Process()
        {
            PushNotificationParameters pushParams = Parameters as PushNotificationParameters;
            AppProvider.NotificationService.PostNotification(pushParams.NotificationName, pushParams.NotificationData);
        }
    }
}