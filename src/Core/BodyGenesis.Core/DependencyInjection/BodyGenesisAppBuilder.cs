using System.Collections;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public class BodyGenesisAppBuilder : IServiceCollection
    {
        private readonly IServiceCollection _services;

        public BodyGenesisAppBuilder(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;

            Configuration = configuration;
        }

        public ServiceDescriptor this[int index] { get => _services[index]; set => _services[index] = value; }

        public IConfiguration Configuration { get; }

        public int Count => _services.Count;

        public bool IsReadOnly => _services.IsReadOnly;

        public void Add(ServiceDescriptor item)
        {
            _services.Add(item);
        }

        public void Clear()
        {
            _services.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return _services.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _services.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _services.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return _services.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _services.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            return _services.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _services.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_services).GetEnumerator();
        }
    }
}
