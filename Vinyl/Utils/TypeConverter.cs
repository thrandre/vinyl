using System;
using System.Collections.Generic;

namespace Vinyl.Utils
{
    public class TypeConverterInstance
    {
        public static TypeConverter Current { get; } = new TypeConverter();
    }

    public class TypeConverter
    {
        private Dictionary<Type, Tuple<Func<object, object>, Func<object, object>, Type>> Converters { get; } =
            new Dictionary<Type, Tuple<Func<object, object>, Func<object, object>, Type>>();

        public TypeConverter AddConverter<T1, T2>(Func<T2, T1> toObject, Func<T1, T2> toValue)
        {
            Converters.Add
            (
                typeof(T1),
                Tuple.Create
                (
                    (Func<object, object>)(o => toObject((T2)Convert.ChangeType(o, typeof(T2)))),
                    (Func<object, object>)(o => Convert.ChangeType(toValue((T1)o), typeof(T2))),
                    typeof(T2)
                )
            );

            return this;
        }

        public bool CanConvert(Type type)
        {
            return Converters.ContainsKey(type);
        }

        public Type ConvertsTo(Type type)
        {
            if (!CanConvert(type))
            {
                throw new ArgumentException(nameof(type));
            }

            return Converters[type].Item3;
        }

        public T ConvertTo<T>(string value)
        {
            return (T)ConvertTo(typeof(T), value);
        }

        public object ConvertTo(Type type, object value)
        {
            if (!CanConvert(type))
            {
                throw new ArgumentException(nameof(type));
            }

            return Converters[type].Item1(value);
        }

        public object ConvertFrom(Type type, object value)
        {
            if (!CanConvert(type))
            {
                throw new ArgumentException(nameof(type));
            }

            return Converters[type].Item2(value);
        }
    }
}