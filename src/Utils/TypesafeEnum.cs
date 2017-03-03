namespace Vinyl.Utils
{
    public interface ITypesafeEnum
    {
        object Key { get; }
    }

    public abstract class TypesafeEnum<T> : ITypesafeEnum
    {
        public T Key { get; }

        object ITypesafeEnum.Key => Key;

        public string Description { get; }

        protected TypesafeEnum(T key, string description)
        {
            Key = key;
            Description = description;
        }

        public override string ToString()
        {
            return Key.ToString();
        }

        public static implicit operator T(TypesafeEnum<T> @enum)
        {
            return @enum.Key;
        }
    }
}
