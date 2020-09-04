using System;

namespace GsriSync.WpfApp.Utils
{
    internal class NotifyProperty<T>
    {
        private readonly Action _handler;
        private T current_value;

        public NotifyProperty(Action handler)
        {
            _handler = handler;
        }

        public T Value
        {
            get => current_value;
            set
            {
                if (Equals(current_value, value)) return;
                current_value = value;
                _handler();
            }
        }
    }
}