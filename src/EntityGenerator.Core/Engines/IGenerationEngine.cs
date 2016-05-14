using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core.Engines
{
    internal interface IGenerationEngine
    {
        void CreateClass(string name);

        void ImplementNotifyPropertyChanged();

        void InheritFromBaseClass(Type baseType);

        void ImplementInterface(Type interfaceType);

        void ImplementProperty(string name, Type type);

        Type CreateType();
    }
}
