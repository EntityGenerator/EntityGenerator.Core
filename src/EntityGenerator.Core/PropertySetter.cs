using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core
{
    /// <summary>
    /// Contains the metadata needed by the <see cref="IInstanceActivator"/> for setting a value in a property.
    /// </summary>
    public class PropertySetter
    {
        /// <summary>
        /// Creates a new instance of <seealso cref="PropertySetter"/>.
        /// </summary>
        public PropertySetter() { }

        /// <summary>
        /// Creates a new instance of <seealso cref="PropertySetter"/> using the given property name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public PropertySetter(string name)
        : this(name, null) { }

        /// <summary>
        /// Creates a new instance of <seealso cref="PropertySetter"/> using the given property name and value.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The new value for the property.</param>
        public PropertySetter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The new value for the property.
        /// </summary>
        public object Value { get; set; }
    }
}
