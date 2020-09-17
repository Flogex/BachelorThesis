using System.Collections;
using System.Collections.Generic;

namespace Flogex.Thesis.IntRest.Conneg
{
    internal class MicrotypeCollection : ICollection<IMicrotypeDescriptor>
    {
        public const string Identifier = "MicroTypes";

        private readonly HashSet<IMicrotypeDescriptor> _microtypes = new HashSet<IMicrotypeDescriptor>(new MicrotypeDescriptorEqualityComparer());

        public int Count => _microtypes.Count;

        public bool IsReadOnly => false;

        public void Add(string category, string identifier)
            => Add(new MicrotypeDescriptor(category, identifier));

        public void Add(IMicrotypeDescriptor descriptor) =>
            _microtypes.Add(descriptor);

        public bool Remove(string category, string identifier)
            => Remove(new MicrotypeDescriptor(category, identifier));

        public bool Remove(IMicrotypeDescriptor descriptor)
            => _microtypes.Remove(descriptor);

        public void Clear()
            => _microtypes.Clear();

        public bool Contains(string category, string identifier)
            => Contains(new MicrotypeDescriptor(category, identifier));

        public bool Contains(IMicrotypeDescriptor item)
            => _microtypes.Contains(item);

        public void CopyTo(IMicrotypeDescriptor[] array, int arrayIndex)
            => _microtypes.CopyTo(array, arrayIndex);

        public IEnumerator<IMicrotypeDescriptor> GetEnumerator()
            => _microtypes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
