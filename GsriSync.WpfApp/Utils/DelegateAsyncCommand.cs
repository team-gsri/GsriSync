using System;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Utils
{
    internal class DelegateAsyncCommand : DelegateCommand
    {
        public delegate void DelegateExceptionEventHandler(object sender, DelegateExceptionEventArgs e);

        public delegate void DelegateFinallyEventhandler(object sender, EventArgs e);

        public event DelegateExceptionEventHandler Exception;

        public event DelegateFinallyEventhandler Finally;

        public DelegateAsyncCommand(Func<object, Task> execute, bool canExecute = true) : base(parameter => execute(parameter), canExecute)
        {
            base.execute = async paramter =>
            {
                try
                {
                    SetCanExecute(false);
                    await execute(paramter);
                }
                catch (Exception ex)
                {
                    Exception?.Invoke(this, new DelegateExceptionEventArgs(ex));
                }
                finally
                {
                    SetCanExecute(true);
                    Finally?.Invoke(this, EventArgs.Empty);
                }
            };
        }
    }
}