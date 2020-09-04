using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GsriSync.WpfApp.Utils
{
    internal class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string property_name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
        }
    }
}