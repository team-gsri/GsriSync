using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GsriSync.WpfApp.Models
{
    public class Manifest
    {
        public ICollection<Addon> Addons { get; set; }

        [JsonIgnore]
        public bool IsInstalled => LastModification != default;

        public DateTimeOffset LastModification { get; set; }

        public Server Server { get; set; }
    }
}
