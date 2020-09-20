using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace GsriSync.WpfApp.Services
{
    internal class ZipClient : IDisposable
    {
        public event UnzipProgressChangedHandler UnzipProgressChanged;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task UnzipArchiveTaskAsync(string zip, string directory)
        {
            using (var archive = ZipFile.OpenRead(zip))
            {
                long current = 0;
                var total = archive.Entries.Sum(entry => entry.CompressedLength);
                foreach (var entry in archive.Entries)
                {
                    current += entry.CompressedLength;
                    if (string.Empty.Equals(entry.Name)) { continue; }
                    var filename = $@"{directory}\{entry.FullName}";
                    var parent = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(parent)) { Directory.CreateDirectory(parent); }
                    await Task.Run(() => entry.ExtractToFile(filename, true));
                    NotifyUnzipProgressChanged(total, current);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private void NotifyUnzipProgressChanged(long archiveSize, long bytesProcessed)
        {
            UnzipProgressChanged?.Invoke(this, new UnzipProgressChangedEventArgs(archiveSize, bytesProcessed));
        }

        internal delegate void UnzipProgressChangedHandler(object sender, UnzipProgressChangedEventArgs args);

        internal class UnzipProgressChangedEventArgs : ProgressChangedEventArgs
        {
            public long ArchiveSize { get; }

            public long BytesProcessed { get; }

            public UnzipProgressChangedEventArgs(long archiveSize, long bytesProcessed)
                : base(Convert.ToInt32(100 * bytesProcessed / archiveSize), null)
            {
                ArchiveSize = archiveSize;
                BytesProcessed = bytesProcessed;
            }
        }
    }
}
