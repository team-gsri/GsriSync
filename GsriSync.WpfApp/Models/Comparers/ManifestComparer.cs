using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GsriSync.WpfApp.Models.Comparers
{
    internal class ManifestComparer : IEqualityComparer<Manifest>
    {
        public bool Equals([AllowNull] Manifest x, [AllowNull] Manifest y)
        {
            if (x == null || y == null) { return object.Equals(x, y); }

            return x.LastModification == y.LastModification
                && x.Addons.SequenceEqual(y.Addons, new AddonHashComparer());
        }

        public int GetHashCode([DisallowNull] Manifest obj)
        {
            throw new NotImplementedException();
        }
    }
}
