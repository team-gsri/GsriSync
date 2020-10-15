using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GsriSync.WpfApp.Models.Comparers
{
    internal class AddonHashComparer : IEqualityComparer<Addon>
    {
        public bool Equals([AllowNull] Addon x, [AllowNull] Addon y)
        {
            if (x == null || y == null) { return object.Equals(x, y); }

            return string.Equals(x.Name, y.Name, StringComparison.InvariantCulture)
                && x.Size == y.Size
                && string.Equals(x.Hash, y.Hash, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode([DisallowNull] Addon obj)
        {
            return HashCode.Combine(obj.Name, obj.Size, obj.Hash);
        }
    }
}
