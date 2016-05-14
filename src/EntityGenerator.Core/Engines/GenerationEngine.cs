using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core.Engines
{
    internal class GenerationEngine : IGenerationEngine
    {
        private const string EntitiesAssemblyName = "EntityGenerator.Core.DynamicEntities";

        public GenerationEngine()
        {
            AssemblyName assemblyName = null;
            AssemblyBuilder assemblyBuilder = null;
            
            assemblyName = new AssemblyName(EntitiesAssemblyName);
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll");
        }

        private ModuleBuilder ModuleBuilder { get; set; }
        private TypeBuilder TypeBuilder { get; set; }

        public void CreateClass(string name)
        {
            TypeBuilder = ModuleBuilder.DefineType(name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Serializable);

            TypeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
        }

        public void ImplementInterface(Type interfaceType)
        {
            PropertyInfo[] properties = null;
            PropertyBuilder propertyBuilder = null;
            FieldBuilder fieldBuilder = null;
            
            TypeBuilder.AddInterfaceImplementation(interfaceType);

            properties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.GetSetMethod(false) != null &&
                    property.GetGetMethod(false) != null)
                {
                    propertyBuilder = TypeBuilder.DefineProperty(property.Name, PropertyAttributes.HasDefault, property.PropertyType, null);

                    fieldBuilder = GenerateFieldFromPropertyName(property.Name, property.PropertyType);

                    propertyBuilder.SetGetMethod(GenerateGetter(property.Name, property.PropertyType, fieldBuilder));
                    propertyBuilder.SetSetMethod(GenerateSetter(property.Name, property.PropertyType, fieldBuilder)); 
                }
            }
        }

        public void ImplementNotifyPropertyChanged()
        {
            
        }

        public void ImplementProperty(string name, Type type)
        {
            PropertyBuilder propertyBuilder = null;
            FieldBuilder fieldBuilder = null;

            propertyBuilder = TypeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);

            fieldBuilder = GenerateFieldFromPropertyName(name, type);
            
            propertyBuilder.SetGetMethod(GenerateGetter(name, type, fieldBuilder));
            propertyBuilder.SetSetMethod(GenerateSetter(name, type, fieldBuilder));
        }

        public void InheritFromBaseClass(Type baseType)
        {
            TypeBuilder.SetParent(baseType);
        }

        public Type CreateType()
        {
            return TypeBuilder.CreateType();
        }

        private FieldBuilder GenerateFieldFromPropertyName(string name, Type type)
        {
            return TypeBuilder.DefineField($"__{name}", type, FieldAttributes.Private);
        }

        private MethodBuilder GenerateGetter(string name, Type type, FieldBuilder fieldBuilder)
        {
            MethodBuilder getPropMethodBuilder = null;
            ILGenerator ilGenerator = null;

            getPropMethodBuilder = TypeBuilder.DefineMethod($"get_{name}",
                                       MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                       type,
                                       Type.EmptyTypes);

            ilGenerator = getPropMethodBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ret);

            return getPropMethodBuilder;

        }

        private MethodBuilder GenerateSetter(string name, Type type, FieldBuilder fieldBuilder)
        {
            MethodBuilder setPropMethodBuilder = null;
            ILGenerator ilGenerator = null;

            setPropMethodBuilder = TypeBuilder.DefineMethod($"set_{name}",
                                       MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                                       null,
                                       new[] { type });

            ilGenerator = setPropMethodBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ret);

            return setPropMethodBuilder;
        }
    }
}
