using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Core
{
    internal class InstanceActivator : IInstanceActivator
    {
        internal InstanceActivator(Type type)
        {
            GeneratedType = type;

            CacheProperties();

            CreateInstantiationDelegate();
        }

        public Type GeneratedType { get; private set; }
        private Dictionary<string, PropertyInfo> Properties { get; set; }
        private Delegate InstantiationDelegate { get; set; }

        public object Create()
        {
            return InstantiationDelegate.DynamicInvoke();
        }

        public object Create(params PropertySetter[] setters)
        {
            object instance = Create();

            foreach (PropertySetter setter in setters)
            {
                Properties[setter.Name].SetValue(instance, setter.Value);
            }

            return instance;
        }

        public TInstance Create<TInstance>()
        {
            return (TInstance)Create();
        }

        public TInstance Create<TInstance>(params PropertySetter[] setters)
        {
            TInstance instance = Create<TInstance>();

            foreach (PropertySetter setter in setters)
            {
                Properties[setter.Name].SetValue(instance, setter.Value);
            }

            return instance;
        }

        public void SetValues(object instance, params PropertySetter[] setters)
        {
            foreach (PropertySetter setter in setters)
            {
                Properties[setter.Name].SetValue(instance, setter.Value);
            }
        }

        public object GetValue(object instance, string propertyName)
        {
            return Properties[propertyName].GetValue(instance);
        }

        public TValue GetValue<TValue>(object instance, string propertyName)
        {
            return (TValue)GetValue(instance, propertyName);
        }
        
        private void CacheProperties()
        {
            PropertyInfo[] properties = GeneratedType.GetProperties();

            Properties = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo property in properties)
            {
                Properties.Add(property.Name, property);
            }
        }

        private void CreateInstantiationDelegate()
        {
            InstantiationDelegate = Expression.Lambda(Expression.New(GeneratedType)).Compile();
        }
    }
}
