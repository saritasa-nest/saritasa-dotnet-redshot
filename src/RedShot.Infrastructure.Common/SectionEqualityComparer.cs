using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RedShot.Abstractions;

namespace RedShot.Infrastructure.Common
{
    public class SectionEqualityComparer : IEqualityComparer<IConfigurationSection>
    {
        public bool Equals([AllowNull] IConfigurationSection x, [AllowNull] IConfigurationSection y)
        {
            return x.UniqueName == y.UniqueName;
        }

        public int GetHashCode([DisallowNull] IConfigurationSection obj)
        {
            return obj.UniqueName.GetHashCode();
        }
    }
}
