using System.Collections.Generic;

namespace Conarh_2016.Core.Services
{
    public enum LogChannel
    {
        None,           			//Unknown channel, can be used for all information
        All,            			//Always printed channel
        Exception,                  //Print exceptions info on this channel
        DataBase,
        Notifications
    }

    public interface ILog
    {
        void WriteLine(LogChannel channel, string format, params object[] args);

        void WriteLine(LogChannel channel, object item);

        void WriteLine(LogChannel channel, string item);
    }

    public abstract class Log : ILog
    {
        protected readonly List<LogChannel> Channels;

        protected Log()
        {
            Channels = new List<LogChannel>
            {
                   LogChannel.Exception,
                LogChannel.Notifications,
                LogChannel.DataBase
            };
        }

        public void WriteLine(LogChannel channel, string format, params object[] args)
        {
            if (channel == LogChannel.All || Channels.Contains(channel))
            {
                Print(string.Format("[{0}]:{1}", channel, string.Format(format, args)));
            }
        }

        public void WriteLine(LogChannel channel, object item)
        {
            if (channel == LogChannel.All || Channels.Contains(channel))
            {
                Print(string.Format("[{0}]:{1}", channel, item));
            }
        }

        public void WriteLine(LogChannel channel, string item)
        {
            if (channel == LogChannel.All || Channels.Contains(channel))
            {
                Print(string.Format("[{0}]:{1}", channel, item));
            }
        }

        protected abstract void Print(string info);
    }
}