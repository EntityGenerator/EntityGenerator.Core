using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntityGenerator.Core.Builders
{
    internal class EntityTypeBuilder
    {
        private const string EntitiesAssemblyName = "EntityGenerator.Core.DynamicEntities";
        private const string PropertyChangtedEventName = "PropertyChanged";

        private readonly Type INotifyPropertyChangedType = typeof(INotifyPropertyChanged);
        private readonly Type PropertyChangedEventHandlerType = typeof(PropertyChangedEventHandler);
        private readonly Type PropertyChangedEventArgsType = typeof(PropertyChangedEventArgs);

        public EntityTypeBuilder()
        {
            AssemblyName assemblyName = null;

            assemblyName = new AssemblyName(EntitiesAssemblyName);
            AssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll");

            GeneratedMethods = new Dictionary<string, MethodBuilder>();
        }

        private AssemblyBuilder AssemblyBuilder { get; set; }
        private ModuleBuilder ModuleBuilder { get; set; }
        private TypeBuilder TypeBuilder { get; set; }
        private Dictionary<string, MethodBuilder> GeneratedMethods { get; set; }

        public void CreateClass(string name)
        {
            TypeBuilder = ModuleBuilder.DefineType($"{EntitiesAssemblyName}.{name}", TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Serializable);

            TypeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
        }

        public void ImplementInterface(Type interfaceType, bool notifyPropertyChanged = false)
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
                    propertyBuilder.SetSetMethod(GenerateSetter(property.Name, property.PropertyType, fieldBuilder, notifyPropertyChanged));
                }
            }
        }

        public void ImplementNotifyPropertyChanged()
        {
            FieldBuilder fieldBuilder = null;
            EventBuilder eventBuilder = null;

            ImplementInterface(INotifyPropertyChangedType);

            eventBuilder = GenerateEvent(PropertyChangtedEventName, PropertyChangedEventHandlerType);

            fieldBuilder = GenerateField(PropertyChangtedEventName, PropertyChangedEventHandlerType);
            
            eventBuilder.SetAddOnMethod(GenerateAdder(PropertyChangtedEventName, PropertyChangedEventHandlerType, fieldBuilder));
            eventBuilder.SetRemoveOnMethod(GenerateRemover(PropertyChangtedEventName, PropertyChangedEventHandlerType, fieldBuilder));

            GeneratedMethods.Add($"On{PropertyChangtedEventName}", GenerateStringParamEventRaiser(PropertyChangtedEventName, fieldBuilder, PropertyChangedEventArgsType, PropertyChangedEventHandlerType));
        }

        public void ImplementProperty(string name, Type type, bool notifyPropertyChanged = false)
        {
            PropertyBuilder propertyBuilder = null;
            FieldBuilder fieldBuilder = null;

            propertyBuilder = TypeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);

            fieldBuilder = GenerateFieldFromPropertyName(name, type);

            propertyBuilder.SetGetMethod(GenerateGetter(name, type, fieldBuilder));
            propertyBuilder.SetSetMethod(GenerateSetter(name, type, fieldBuilder, notifyPropertyChanged));
        }

        public void InheritFromBaseClass(Type baseType)
        {
            TypeBuilder.SetParent(baseType);
        }

        public Type CreateType()
        {
            return TypeBuilder.CreateType();
        }

        private FieldBuilder GenerateField(string name, Type type)
        {
            return TypeBuilder.DefineField(name, type, FieldAttributes.Private);
        }

        private FieldBuilder GenerateFieldFromPropertyName(string name, Type type)
        {
            return TypeBuilder.DefineField($"_{name}", type, FieldAttributes.Private);
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

        private MethodBuilder GenerateSetter(string name, Type type, FieldBuilder fieldBuilder, bool notifyPropertyChanged = false)
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

            if (notifyPropertyChanged)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldstr, name);
                ilGenerator.Emit(OpCodes.Call, GeneratedMethods[$"On{PropertyChangtedEventName}"]); 
            }

            ilGenerator.Emit(OpCodes.Ret);

            return setPropMethodBuilder;
        }

        private EventBuilder GenerateEvent(string name, Type type)
        {
            return TypeBuilder.DefineEvent(name, EventAttributes.None, type);
        }

        private MethodBuilder GenerateAdder(string name, Type type, FieldBuilder fieldBuilder)
        {
            MethodBuilder addEventMethodBuilder = null;
            ILGenerator ilGenerator = null;
            Label loopLabel = default(Label);

            addEventMethodBuilder = TypeBuilder.DefineMethod($"add_{name}", 
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.NewSlot |
                MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, 
                null, 
                new[] { type });

            ilGenerator = addEventMethodBuilder.GetILGenerator();

            loopLabel = ilGenerator.DefineLabel();

            ilGenerator.DeclareLocal(PropertyChangedEventHandlerType);
            ilGenerator.DeclareLocal(PropertyChangedEventHandlerType);
            ilGenerator.DeclareLocal(PropertyChangedEventHandlerType);

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.MarkLabel(loopLabel);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Stloc_1);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.EmitCall(OpCodes.Call, typeof(Delegate).GetMethod("Combine", new[] { typeof(Delegate), typeof(Delegate) }), Type.EmptyTypes);
            ilGenerator.Emit(OpCodes.Castclass, PropertyChangedEventHandlerType);
            ilGenerator.Emit(OpCodes.Stloc_2);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldflda, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ldloc_2);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.EmitCall(OpCodes.Call, 
                typeof(Interlocked).GetMethods()
                    .Where(method => method.Name == "CompareExchange" && method.IsGenericMethod)
                    .Single()
                    .MakeGenericMethod(PropertyChangedEventHandlerType), null);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Bne_Un_S, loopLabel);
            ilGenerator.Emit(OpCodes.Ret);

            return addEventMethodBuilder;
        }

        private MethodBuilder GenerateRemover(string name, Type type, FieldBuilder fieldBuilder)
        {
            MethodBuilder removeEventMethodBuilder = null;
            ILGenerator ilGenerator = null;
            Label loopLabel = default(Label);

            removeEventMethodBuilder = TypeBuilder.DefineMethod($"remove_{name}", 
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.NewSlot |
                MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, 
                null, 
                new[] { type });

            ilGenerator = removeEventMethodBuilder.GetILGenerator();

            loopLabel = ilGenerator.DefineLabel();

            ilGenerator.DeclareLocal(PropertyChangedEventHandlerType);
            ilGenerator.DeclareLocal(PropertyChangedEventHandlerType);
            ilGenerator.DeclareLocal(PropertyChangedEventHandlerType);

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.MarkLabel(loopLabel);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Stloc_1);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.EmitCall(OpCodes.Call, typeof(Delegate).GetMethod("Remove", new[] { typeof(Delegate), typeof(Delegate) }), Type.EmptyTypes);
            ilGenerator.Emit(OpCodes.Castclass, PropertyChangedEventHandlerType);
            ilGenerator.Emit(OpCodes.Stloc_2);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldflda, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ldloc_2);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.EmitCall(OpCodes.Call,
                typeof(Interlocked).GetMethods()
                    .Where(method => method.Name == "CompareExchange" && method.IsGenericMethod)
                    .Single()
                    .MakeGenericMethod(PropertyChangedEventHandlerType), null);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Bne_Un_S, loopLabel);
            ilGenerator.Emit(OpCodes.Ret);


            return removeEventMethodBuilder;
        }

        private MethodBuilder GenerateStringParamEventRaiser(string name,
            FieldBuilder fieldBuilder,
            Type eventArgsType,
            Type eventHandlerType)
        {
            MethodBuilder eventRaiserMethodBuilder = null;
            ILGenerator ilGenerator = null;
            Label exitLabel = default(Label);
            Type[] paramTypes = new[] { typeof(string) };

            eventRaiserMethodBuilder = TypeBuilder.DefineMethod($"On{name}", 
                MethodAttributes.Private | MethodAttributes.HideBySig, 
                null, 
                paramTypes);

            ilGenerator = eventRaiserMethodBuilder.GetILGenerator();

            ilGenerator.DeclareLocal(PropertyChangedEventHandlerType);
            ilGenerator.DeclareLocal(PropertyChangedEventArgsType);
            
            exitLabel = ilGenerator.DefineLabel();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ldnull);
            ilGenerator.Emit(OpCodes.Beq, exitLabel);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Newobj, eventArgsType.GetConstructor(paramTypes));
            ilGenerator.Emit(OpCodes.Stloc_1);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.EmitCall(OpCodes.Call, eventHandlerType.GetMethod("Invoke"), Type.EmptyTypes);
            ilGenerator.MarkLabel(exitLabel);
            ilGenerator.Emit(OpCodes.Ret);

            return eventRaiserMethodBuilder;
        }
    }
}
