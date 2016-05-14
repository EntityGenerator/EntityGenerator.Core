using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core
{
    internal class InstanceBuilder : IInstanceBuilder
    {
        internal InstanceBuilder(Type type, IEnumerable<PropertyDefinition> properties)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
