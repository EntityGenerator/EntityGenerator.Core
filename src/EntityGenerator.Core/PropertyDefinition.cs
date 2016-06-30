using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core
{
    /// <summary>
    /// Contains the metadata needed by the <see cref="ITypeGenerator"/> for generating a property.
    /// </summary>
    public abstract class PropertyDefinition
    {
        /// <summary>
        /// Creates a new instance of <seealso cref="PropertyDefinition"/>.
        /// </summary>
        public PropertyDefinition() { }

        /// <summary>
        /// Creates a new instance of <seealso cref="PropertyDefinition"/> using the given <paramref name="name"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The <seealso cref="Type"/> of the property.</param>
        public PropertyDefinition(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The <see cref="Type"/> of the property.
        /// </summary>
        public Type Type { get; private set; }
    }

    /// <summary>
    /// Contains the metadata needed by the <see cref="ITypeGenerator"/> for generating a property.
    /// </summary>
    /// <typeparam name="TType">The <seealso cref="Type"/> of the property.</typeparam>
    public class PropertyDefinition<TType> : PropertyDefinition
    {
        /// <summary>
        /// Creates a new instance of <seealso cref="PropertyDefinition"/> using the given <paramref name="name"/> and <typeparamref name="TType"/> as the <see cref="Type"/> of the property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public PropertyDefinition(string name)
            : base(name, typeof(TType)) { }
    }
}
