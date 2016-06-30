using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using EntityGenerator.Core.Builders;
using System.ComponentModel;

namespace EntityGenerator.Core
{
    /// <summary>
    /// Implments the <seealso cref="ITypeGenerator"/> interface.
    /// </summary>
    public class TypeGenerator : ITypeGenerator
    {
        /// <summary>
        /// Creates a new instance of <seealso cref="TypeGenerator"/>.
        /// </summary>
        public TypeGenerator() { }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder(string typeName, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, false, properties, null, null);
        }
        
        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <seealso cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder(string typeName, bool asNotifyPropertyChanged, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, true, properties, null, null);
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TInterface>(string typeName)
        {
            return GenerateBuilder(typeName, false, null, null, new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TInterface>(string typeName, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, false, properties, null, new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TInterface>(string typeName, bool asNotifyPropertyChanged)
        {
            return GenerateBuilder(typeName, asNotifyPropertyChanged, null, null, new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TInterface>(string typeName, bool asNotifyPropertyChanged, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, asNotifyPropertyChanged, properties, null, new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName)
        {
            return GenerateBuilder(typeName, false, null, typeof(TBase), new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, false, properties, typeof(TBase), new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, bool asNotifyPropertyChanged)
        {
            return GenerateBuilder(typeName, asNotifyPropertyChanged, null, typeof(TBase), new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, bool asNotifyPropertyChanged, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, asNotifyPropertyChanged, properties, typeof(TBase), new[] { typeof(TInterface) });
        }

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <param name="baseType">A class that should be inherited by the generated <see cref="Type"/>.</param>
        /// <param name="interfaces">A list of interfaces that should be implmeneted by the generated <seealso cref="Type"/>.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        public IInstanceActivator GenerateBuilder(string typeName, bool asNotifyPropertyChanged, IEnumerable<PropertyDefinition> properties, Type baseType, IEnumerable<Type> interfaces)
        {
            EntityTypeBuilder engine = null;
            InstanceActivator builder = null;

            engine = new EntityTypeBuilder();

            engine.CreateClass(typeName);

            if (asNotifyPropertyChanged)
            {
                engine.ImplementNotifyPropertyChanged();
            }

            if (baseType != null)
            {
                engine.InheritFromBaseClass(baseType);
            }

            if (interfaces != null)
            {
                foreach (Type interfaceType in interfaces)
                {
                    engine.ImplementInterface(interfaceType, asNotifyPropertyChanged);
                }
            }

            if (properties != null)
            {
                foreach (PropertyDefinition property in properties)
                {
                    engine.ImplementProperty(property.Name, property.Type, asNotifyPropertyChanged);
                }
            }

            builder = new InstanceActivator(engine.CreateType());

            return builder;
        }
    }
}
