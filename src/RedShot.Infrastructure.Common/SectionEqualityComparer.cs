using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RedShot.Infrastructure.Abstractions;

namespace RedShot.Infrastructure.Common
{
    public class SectionEqualityComparer : IEqualityComparer<IConfigurationOption>
    {
        public bool Equals([AllowNull] IConfigurationOption x, [AllowNull] IConfigurationOption y)
        {
            return x.UniqueName == y.UniqueName;
        }

        public int GetHashCode([DisallowNull] IConfigurationOption obj)
        {
            return obj.UniqueName.GetHashCode();
        }
    }
}
