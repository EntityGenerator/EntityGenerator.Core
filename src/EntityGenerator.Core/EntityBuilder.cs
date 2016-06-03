using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using EntityGenerator.Core.Engines;

namespace EntityGenerator.Core
{
    public class EntityBuilder : IEntityBuilder
    {
        public EntityBuilder() { }

        public IInstanceBuilder GenerateBuilder(string typeName,
            bool asNotifyPropertyChanged,
            IEnumerable<PropertyDefinition> properties = null,
            Type baseType = null,
            IEnumerable<Type> interfaces = null)
        {
            IGenerationEngine engine = null;
            InstanceBuilder builder = null;

            engine = new GenerationEngine();

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

            builder = new InstanceBuilder(engine.CreateType(), properties);

            return builder;
        }
    }
}
