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
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false);

            Assert.AreEqual("TestType", instanceBuilder?.GeneratedType?.Name);
        }

        [TestMethod]
        public void CreateTypeWithProperty()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, new[] { new PropertyDefinition<string>("TestProperty") });

            Assert.IsNotNull(instanceBuilder?.GeneratedType?.GetProperty("TestProperty"));
        }

        [TestMethod]
        public void SetGetGeneratedProperty()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            PropertyInfo propertyInfo = null;
            object instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, new[] { new PropertyDefinition<string>("TestProperty") });
            propertyInfo = instanceBuilder.GeneratedType.GetProperty("TestProperty");
            instance = Activator.CreateInstance(instanceBuilder.GeneratedType);

            propertyInfo.SetValue(instance, "TestValue");

            Assert.AreEqual("TestValue", propertyInfo.GetValue(instance));
        }

        [TestMethod]
        public void CreateFromBaseType()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            MockBaseType instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, null, typeof(MockBaseType), null);

            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as MockBaseType;

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void SetGetFromBaseType()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            MockBaseType instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, null, typeof(MockBaseType), null);
            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as MockBaseType;

            instance.BaseProperty = "TestBaseValue";

            Assert.AreEqual("TestBaseValue", instance.BaseProperty);
        }

        [TestMethod]
        public void CreateFromInterface()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            IMockInterface instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, null, null, new[] { typeof(IMockInterface) });

            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as IMockInterface;

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void CreateWithNotifyPropertyChanged()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            INotifyPropertyChanged instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true);

            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as INotifyPropertyChanged;

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void AddNotifyPropertyChangedHandler()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            INotifyPropertyChanged instance = null;
            Exception exception = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });

            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as INotifyPropertyChanged;

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
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            INotifyPropertyChanged instance = null;
            Exception exception = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });

            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as INotifyPropertyChanged;

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
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            INotifyPropertyChanged instance = null;
            MethodInfo methodInfo = null;
            Exception exception = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });
            methodInfo = instanceBuilder.GeneratedType.GetMethod("OnPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance);

            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as INotifyPropertyChanged;

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
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            PropertyInfo propertyInfo = null;
            INotifyPropertyChanged instance = null;
            TaskCompletionSource<string> handlerSource = new TaskCompletionSource<string>();
            
            instanceBuilder = entityBuilder.GenerateBuilder("TestType", true, new[] { new PropertyDefinition<string>("TestProperty") });
            propertyInfo = instanceBuilder.GeneratedType.GetProperty("TestProperty");

            instance = Activator.CreateInstance(instanceBuilder.GeneratedType) as INotifyPropertyChanged;
            instance.PropertyChanged += (sender, e) => handlerSource.SetResult(e.PropertyName);
            
            propertyInfo.SetValue(instance, "TestValue");

            handlerSource.Task.Wait(1000);
            
            Assert.IsTrue(handlerSource.Task.IsCompleted);
        }
    }
}
