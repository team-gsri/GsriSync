using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Utils
{
    internal class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory) :
            base(() => Task.FromResult(valueFactory()))
        { }

        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(async () => await taskFactory())
        { }

        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }
    }
}
