using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using EntityGenerator.Core.Tests.Mocks;
using System.ComponentModel;
using System.Threading.Tasks;

namespace EntityGenerator.Core.Tests
{
    [TestClass]
    public class EntityBuilderTest
    {
        [TestMethod]
        public void CreateEmptyType()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false);

            Assert.AreEqual("TestType", instanceBuilder?.Type?.Name);
        }

        [TestMethod]
        public void CreateTypeWithProperty()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, new[] { new PropertyDefinition<string>("TestProperty") });

            Assert.IsNotNull(instanceBuilder?.Type?.GetProperty("TestProperty"));
        }

        [TestMethod]
        public void SetGetGeneratedProperty()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            PropertyInfo propertyInfo = null;
            object instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, new[] { new PropertyDefinition<string>("TestProperty") });
            propertyInfo = instanceBuilder.Type.GetProperty("TestProperty");
            instance = Activator.CreateInstance(instanceBuilder.Type);

            propertyInfo.SetValue(instance, "TestValue");

            Assert.AreEqual("TestValue", propertyInfo.GetValue(instance));
        }

        [TestMethod]
        public void CreateFromBaseType()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            MockBaseType instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, baseType: typeof(MockBaseType));

            instance = Activator.CreateInstance(instanceBuilder.Type) as MockBaseType;

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void SetGetFromBaseType()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            MockBaseType instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, baseType: typeof(MockBaseType));
            instance = Activator.CreateInstance(instanceBuilder.Type) as MockBaseType;

            instance.BaseProperty = "TestBaseValue";

            Assert.AreEqual("TestBaseValue", instance.BaseProperty);
        }

        [TestMethod]
        public void CreateFromInterface()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            IMockInterface instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, interfaces: new[] { typeof(IMockInterface) });

            instance = Activator.CreateInstance(instanceBuilder.Type) as IMockInterface;

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void CreateWithNotifyPropertyChanged()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            INotifyPropertyChanged instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true);

            instance = Activator.CreateInstance(instanceBuilder.Type) as INotifyPropertyChanged;

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void AddNotifyPropertyChangedHandler()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            INotifyPropertyChanged instance = null;
            Exception exception = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });

            instance = Activator.CreateInstance(instanceBuilder.Type) as INotifyPropertyChanged;

            try
            {
                instance.PropertyChanged += (sender, e) => { };
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
        }

        [TestMethod]
        public void RemoveNotifyPropertyChangedHandler()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            INotifyPropertyChanged instance = null;
            Exception exception = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });

            instance = Activator.CreateInstance(instanceBuilder.Type) as INotifyPropertyChanged;

            instance.PropertyChanged += (sender, e) => { };

            try
            {
                instance.PropertyChanged -= (sender, e) => { };
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
        }

        [TestMethod]
        public void ExecuteOnPropertyChangedRaiser()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            INotifyPropertyChanged instance = null;
            MethodInfo methodInfo = null;
            Exception exception = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });
            methodInfo = instanceBuilder.Type.GetMethod("OnPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance);

            instance = Activator.CreateInstance(instanceBuilder.Type) as INotifyPropertyChanged;

            instance.PropertyChanged += (sender, e) => { };

            try
            {
                methodInfo.Invoke(instance, new[] { "Test Value" });
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception);
        }

        [TestMethod]
        public void SetWithNotifyPropertyChanged()
        {
            EntityBuilder entityBuilder = new EntityBuilder();
            IInstanceBuilder instanceBuilder = null;
            PropertyInfo propertyInfo = null;
            INotifyPropertyChanged instance = null;
            TaskCompletionSource<string> handlerSource = new TaskCompletionSource<string>();
            
            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });
            propertyInfo = instanceBuilder.Type.GetProperty("TestProperty");

            instance = Activator.CreateInstance(instanceBuilder.Type) as INotifyPropertyChanged;
            instance.PropertyChanged += (sender, e) => handlerSource.SetResult(e.PropertyName);
            
            propertyInfo.SetValue(instance, "TestValue");

            handlerSource.Task.Wait(1000);
            
            Assert.IsTrue(handlerSource.Task.IsCompleted);
        }
    }
}
