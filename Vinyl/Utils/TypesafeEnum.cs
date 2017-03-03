using System.ComponentModel;

namespace Vinyl.Utils
{
    [TypeConverter(typeof(BoxedTypeConverter))]
    public abstract class TypesafeEnum<T>
    {
        public T Value { get; }

        public string Description { get; }

        protected TypesafeEnum(T value, string description)
        {
            Value = value;
            Description = description;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(TypesafeEnum<T> @enum)
        {
            return @enum.Value;
        }
    }
}
