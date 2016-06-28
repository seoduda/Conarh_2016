
using System;
using System.Threading;

namespace Conarh_2016.Core
{
    public class Singletone<T> where T : class
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() => (T)Activator.CreateInstance(typeof(T)), LazyThreadSafetyMode.ExecutionAndPublication);

        public static T Instance
        {
            get { return instance.Value; }
        }

    }
}
