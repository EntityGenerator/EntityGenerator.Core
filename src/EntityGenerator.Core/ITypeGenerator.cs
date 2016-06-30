using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core
{
    /// <summary>
    /// Generates a <see cref="Type"/> using provided parameters and creates a <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <see cref="Type"/>.
    /// </summary>
    public interface ITypeGenerator
    {
        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder(string typeName, params PropertyDefinition[] properties);
        
        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <seealso cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder(string typeName, bool asNotifyPropertyChanged, params PropertyDefinition[] properties);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TInterface>(string typeName);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TInterface>(string typeName, bool asNotifyPropertyChanged);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TInterface>(string typeName, params PropertyDefinition[] properties);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TInterface>(string typeName, bool asNotifyPropertyChanged, params PropertyDefinition[] properties);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, bool asNotifyPropertyChanged);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, params PropertyDefinition[] properties);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <typeparam name="TBase">A class that should be inherited by the generated <see cref="Type"/>.</typeparam>
        /// <typeparam name="TInterface">An interface that should be implmented by the generated <see cref="Type"/>.</typeparam>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, bool asNotifyPropertyChanged, params PropertyDefinition[] properties);

        /// <summary>
        /// Generates a <seealso cref="Type"/> using the provided parameters and returns an <seealso cref="IInstanceActivator"/> that can be used to create instances of the generated <seealso cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The name that will be used when generating the <seealso cref="Type"/>.</param>
        /// <param name="asNotifyPropertyChanged">A <seealso cref="bool">boolean</seealso> value indicating whether the generated <seealso cref="Type"/> should implement the <see cref="INotifyPropertyChanged"/> interface.</param>
        /// <param name="properties">The list of <seealso cref="PropertyDefinition"/> instances that define the properties that generated type will have.</param>
        /// <param name="baseType">A class that should be inherited by the generated <see cref="Type"/>.</param>
        /// <param name="interfaces">A list of interfaces that should be implmeneted by the generated <seealso cref="Type"/>.</param>
        /// <returns>An instance of <seealso cref="IInstanceActivator"/>.</returns>
        IInstanceActivator GenerateBuilder(string typeName, bool asNotifyPropertyChanged, IEnumerable<PropertyDefinition> properties, Type baseType, IEnumerable<Type> interfaces);
    }
}
