using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using EntityGenerator.Core.Tests.Mocks;

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
    }
}
