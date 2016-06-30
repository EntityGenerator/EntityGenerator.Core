using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace EntityGenerator.Core.Tests
{
    [TestClass]
    public class InstanceBuilderTest
    {
        [TestMethod]
        public void CreateInstanceFromEmptyType()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false);

            Assert.IsNotNull(instanceBuilder.Create());
        }

        [TestMethod]
        public void CreateInstanceWithPropertyValue()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            object instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, new[] { new PropertyDefinition<string>("TestProperty") });

            instance = instanceBuilder.Create(new PropertySetter("TestProperty", "TestValue"));

            Assert.AreEqual("TestValue", instanceBuilder?.GeneratedType?.GetProperty("TestProperty").GetValue(instance));
        }

        [TestMethod]
        public void SetPropertyValue()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            object instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, new[] { new PropertyDefinition<string>("TestProperty") });

            instance = instanceBuilder.Create();

            instanceBuilder.SetValues(instance, new PropertySetter("TestProperty", "TestValue"));

            Assert.AreEqual("TestValue", instanceBuilder?.GeneratedType?.GetProperty("TestProperty").GetValue(instance));
        }

        [TestMethod]
        public void GetPropertyValue()
        {
            ITypeGenerator entityBuilder = new TypeGenerator();
            IInstanceActivator instanceBuilder = null;
            object instance = null;

            instanceBuilder = entityBuilder.GenerateBuilder("TestType", false, new[] { new PropertyDefinition<string>("TestProperty") });

            instance = instanceBuilder.Create();

            instanceBuilder.SetValues(instance, new PropertySetter("TestProperty", "TestValue"));

            Assert.AreEqual("TestValue", instanceBuilder.GetValue<string>(instance, "TestProperty"));
        }
    }
}
