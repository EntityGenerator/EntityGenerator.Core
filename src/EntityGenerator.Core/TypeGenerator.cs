using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using EntityGenerator.Core.Builders;

namespace EntityGenerator.Core
{
    public class TypeGenerator : ITypeGenerator
    {
        public TypeGenerator() { }

        public IInstanceActivator GenerateBuilder(string typeName, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, false, properties, null, null);
        }

        public IInstanceActivator GenerateBuilder(string typeName, bool asNotifyPropertyChanged, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, true, properties, null, null);
        }

        public IInstanceActivator GenerateBuilder<TInterface>(string typeName)
        {
            return GenerateBuilder(typeName, false, null, null, new[] { typeof(TInterface) });
        }

        public IInstanceActivator GenerateBuilder<TInterface>(string typeName, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, false, properties, null, new[] { typeof(TInterface) });
        }

        public IInstanceActivator GenerateBuilder<TInterface>(string typeName, bool asNotifyPropertyCahnged)
        {
            return GenerateBuilder(typeName, asNotifyPropertyCahnged, null, null, new[] { typeof(TInterface) });
        }

        public IInstanceActivator GenerateBuilder<TInterface>(string typeName, bool asNotifyPropertyCahnged, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, asNotifyPropertyCahnged, properties, null, new[] { typeof(TInterface) });
        }

        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName)
        {
            return GenerateBuilder(typeName, false, null, typeof(TBase), new[] { typeof(TInterface) });
        }

        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, false, properties, typeof(TBase), new[] { typeof(TInterface) });
        }

        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, bool asNotifyPropertyCahnged)
        {
            return GenerateBuilder(typeName, asNotifyPropertyCahnged, null, typeof(TBase), new[] { typeof(TInterface) });
        }

        public IInstanceActivator GenerateBuilder<TBase, TInterface>(string typeName, bool asNotifyPropertyCahnged, params PropertyDefinition[] properties)
        {
            return GenerateBuilder(typeName, asNotifyPropertyCahnged, properties, typeof(TBase), new[] { typeof(TInterface) });
        }

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
