using System;
using System.Runtime.Serialization;

namespace GsriSync.WpfApp.Repositories.Errors
{
    [Serializable]
    public class RepositoryException<T> : InvalidOperationException
        where T : struct
    {
        public T Error { get; private set; }

        public RepositoryException(T error)
            : base()
        {
            GuardEnumType();
            Error = error;
        }

        public RepositoryException(string message, T errror)
            : base(message)
        {
            GuardEnumType();
            Error = errror;
        }

        public RepositoryException(string message, Exception innerException, T error)
            : base(message, innerException)
        {
            GuardEnumType();
            Error = error;
        }

        protected RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new ArgumentNullException(nameof(info)); }
            info.AddValue(nameof(Error), Enum.GetName(typeof(T), this.Error));
            base.GetObjectData(info, context);
        }

        private void GuardEnumType()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException();
            }
        }
    }
}
