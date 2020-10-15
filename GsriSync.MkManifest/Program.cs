using GsriSync.WpfApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace GsriSync.MkManifest
{
    internal static class Program
    {
        private static IEnumerable<Addon> GetFiles(string local_directory, string remote_base)
        {
            using var md5 = MD5.Create();
            foreach (var file in Directory.GetFiles(local_directory, "*", SearchOption.AllDirectories))
            {
                var relative = Path.GetRelativePath(local_directory, file);
                var relative_safe = relative.Replace("\\", "/");
                var size = Convert.ToUInt64(new FileInfo(file).Length);
                var remote_url = $"{remote_base}/{relative_safe}";
                var hash = BitConverter.ToString(md5.ComputeHash(File.OpenRead(file))).Replace("-", string.Empty).ToLower();
                yield return new Addon { Name = relative, Size = size, Hash = hash, RemoteUrl = remote_url };
            }
        }

        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                PrintHelp();
                return 1;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Directory not found");
                PrintHelp();
                return 2;
            }

            if (!Uri.IsWellFormedUriString(args[1], UriKind.Absolute))
            {
                Console.WriteLine("Url is not valid");
                PrintHelp();
                return 3;
            }

            var files = GetFiles(args[0], args[1]);
            var server = new Server { Name = "Xeon", Hostname = "arma.gsri.team", Port = 2502, TeamspeakUrl = "ts3server://ts.gsri.team", Addons = new[] { "@GSRI" } };
            var manifest = new Manifest { Addons = files, Server = server, LastModification = DateTimeOffset.Now };
            var data = JsonSerializer.Serialize(manifest);
            Console.WriteLine(data);
            return 0;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: MkManifest <mod_path> <service_uri>");
            Console.WriteLine("  mod_path:    path to a directory containing files to include in the manifest");
            Console.WriteLine("  service_uri: base url where to download the files remotely");
        }
    }
}
