using System;

namespace GsriSync.WpfApp.Utils
{
    public class DelegateExceptionEventArgs : EventArgs
    {
        public DelegateExceptionEventArgs(Exception ex)
        {
            Exception = ex;
        }

        public Exception Exception { get; }
    }
}