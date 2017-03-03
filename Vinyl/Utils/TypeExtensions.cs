using System;
using System.Linq;
using System.Reflection;

namespace Vinyl.Utils
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfGenericType(this Type target, Type superType)
        {
            return
                target.GetTypeInfo()
                    .Then(
                        ti =>
                            ti.BaseType.IsConstructedGenericType &&
                            ti.BaseType.GetGenericTypeDefinition() == superType.GetGenericTypeDefinition());
        }

        public static Type GetGenericTypeArgumentOfBaseClass(this Type target)
        {
            return target.GetTypeInfo()
                .BaseType 
                .GetGenericArguments()
                .First();
        }
    }
}