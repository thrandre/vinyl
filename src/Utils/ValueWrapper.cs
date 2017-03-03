namespace Vinyl.Utils
{
    public interface IValueWrapper
    {
        object Value { get; }
    }

    public abstract class ValueWrapper<T> : IValueWrapper
    {
        public T Value { get; }

        object IValueWrapper.Value => Value;

        protected ValueWrapper(T value)
        {
            Value = value;
        }

        public bool Equals(ValueWrapper<T> obj)
        {
            return obj != null && Value.Equals(obj.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ValueWrapper<T>);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(ValueWrapper<T> valueWrapper)
        {
            return valueWrapper.Value;
        }
    }
}