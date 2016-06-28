using System;
using System.Net;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using Conarh_2016.Core.Commands;
using Conarh_2016.Core.Exceptions;
using Conarh_2016.Core.Exceptions.Server;
using Conarh_2016.Core.Net;
using Conarh_2016.Application.DataAccess;
using Conarh_2016.Core.Services;
using Conarh_2016.Core;

namespace Conarh_2016.Application.Commands.Server
{
    public sealed class LogInParameters : ServerCommandParameters
    {
        public readonly string Server;
        public readonly User User;

        public LogInParameters(ICommandHandler handler, string userName, string password,
            bool onlineMode, Action<ServerCommand> onSuccess = null, Action<ServerCommand> onFault = null)
            : base(handler, onlineMode, onSuccess, onFault, true, new NetworkCredential(userName, password))
        {
            User = new User {Name = userName};
        }
    }

    public sealed class LogInCommand : ServerCommand
    {
        public const double DefaultLoginTimeout = 10000;

        private LogInParameters LoginParameters
        {
            get { return Parameters as LogInParameters; }
            }

        protected override void PingServerToCheckConnection()
        {
        }

        protected override void ProcessOnlineStrategy()
        {
            if (LoginParameters != null)
            {
                try
                {
					WebClient.GetStringAsync(QueryBuilder.Instance.GetLoginQuery(), WebClient.JsonMimeType, TimeSpan.FromMilliseconds(DefaultLoginTimeout)).Wait();
                    
                    DbClient.Instance.SaveUser(LoginParameters.User).Wait();
                    AppProvider.UserData.SetLoginData(LoginParameters.Credentials);
                }
                catch (FormatException)
                {
                    throw new ResourceNotFoundException(LoginParameters.Server);
                }
            }
        }

        protected override void HandleNotFoundException(CoreException exception)
        {
            base.HandleNotFoundException(exception);
            CommandHandler.Handle(new ServerNotFound(this, exception));
        }

        protected override void ProcessOfflineStrategy()
        {
            AppProvider.Log.WriteLine(LogChannel.All, "{0} : ProcessOfflineStrategy", GetType());
        }
    }
}