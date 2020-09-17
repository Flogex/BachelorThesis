using System;
using System.Collections.Generic;

namespace Flogex.Thesis.IntRest
{
    internal class MicrotypeDescriptorEqualityComparer : IEqualityComparer<IMicrotypeDescriptor>
    {
        public bool Equals(IMicrotypeDescriptor? x, IMicrotypeDescriptor? y)
        {
            if (x is null)
                if (y is null)
                    return true;
                else
                    return false;
            else
                return x.Equals(y);
        }

        public int GetHashCode(IMicrotypeDescriptor obj)
            => HashCode.Combine(obj.Category, obj.Identifier);
    }
}
