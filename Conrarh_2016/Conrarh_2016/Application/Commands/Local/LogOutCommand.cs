using System;
using Conarh_2016.Core.Commands;
using Conarh_2016.Core;

namespace Conarh_2016.Application.Commands.Local
{
    public sealed class LogOutParameters : LocalCommandParameters
    {
        public LogOutParameters(ICommandHandler handler, Action<LocalCommand> onSuccess = null, Action<LocalCommand> onFault = null)
            : base(handler, onSuccess, onFault)
        {
        }
    }

    public sealed class LogOutCommand : LocalCommand
    {
        protected override void Process()
        {
            AppProvider.UserData.ClearLoginData();
        }
    }
}