using System;

namespace GsriSync.WpfApp.Events
{
    public class InstallProgressChangedEventArgs : EventArgs
    {
        public string Filename { get; }

        public int Progress { get; }

        public string Stage { get; }

        public InstallProgressChangedEventArgs(string filename, string stage, int progress)
        {
            Filename = filename;
            Stage = stage;
            Progress = progress;
        }
    }
}
