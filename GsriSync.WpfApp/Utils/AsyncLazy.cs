using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Utils
{
    internal class AsyncLazy<T>
    {
        private readonly Func<Task<T>> _valueFactory;

        public bool IsValueCreated { get; private set; } = false;

        public T Value { get; private set; }

        public AsyncLazy(Func<Task<T>> valueFactory)
        {
            _valueFactory = valueFactory;
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            var awaiter = _valueFactory().GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                Value = awaiter.GetResult();
                IsValueCreated = true;
            });
            return awaiter;
        }
    }
}
