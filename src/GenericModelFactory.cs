using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vinyl
{
    public class GenericModelFactory
    {
        private Dictionary<Type, Func<Type, Type, Func<object, object>>> Factories { get; } = new Dictionary<Type, Func<Type, Type, Func<object, object>>>();
        private ConcurrentDictionary<Type, Func<object, object>> FactoryCache { get; } = new ConcurrentDictionary<Type, Func<object, object>>();

        public void AddMapping(Type baseType, Func<Type, Type, Func<object, object>> creator)
        {
            Factories.Add(baseType.GetGenericTypeDefinition(), creator);
        }

        private Func<object, object> GetInstantiator(Type concreteType)
        {
            return FactoryCache.GetOrAdd
            (
                concreteType,
                ctype =>
                {
                    var typeInfo = ctype.GetTypeInfo();
                    var baseType = typeInfo.BaseType.GetGenericTypeDefinition();
                    var genericType = typeInfo.BaseType.GetGenericArguments().First();

                    if (!Factories.ContainsKey(baseType))
                    {
                        throw new InvalidCastException("Unable to map type.");
                    }

                    return Factories[baseType].Invoke(ctype, genericType);
                }
            );
        }

        public bool CanMap(Type concreteType, Type baseType = null)
        {
            if (FactoryCache.ContainsKey(concreteType))
            {
                return true;
            }

            try
            {
                GetInstantiator(concreteType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object Map(Type concreteType, object value)
        {
            return GetInstantiator(concreteType).Invoke(value);
        }
    }
}