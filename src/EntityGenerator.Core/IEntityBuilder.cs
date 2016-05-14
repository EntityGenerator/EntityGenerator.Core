using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core
{
    public interface IEntityBuilder
    {
        IInstanceBuilder GenerateBuilder(string typeName, 
            bool asNotifyPropertyChanged, 
            IEnumerable<PropertyDefinition> properties = null, 
            Type baseType = null,
            IEnumerable<Type> interfaces = null);
    }
}
