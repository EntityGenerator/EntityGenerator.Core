using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core
{
    public abstract class PropertyDefinition
    {
        public PropertyDefinition() { }

        public PropertyDefinition(string name, Type type)
            : this(name, type, null) { }

        public PropertyDefinition(string name, Type type, object defaultValue)
        {
            Name = name;
            Type = type;
            DefaultValue = defaultValue;
        }

        public string Name { get; private set; }
        public Type Type { get; private set; }
        public object DefaultValue { get; set; }
    }

    public class PropertyDefinition<TType> : PropertyDefinition
    {
        public PropertyDefinition(string name)
            : this(name, default(TType)) { }

        public PropertyDefinition(string name, object defaultValue)
            : base(name, typeof(TType), defaultValue) { }
    }
}
