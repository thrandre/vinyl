using System;

namespace Vinyl.Utils
{
    public static class FunctionalExtensions
    {
        public static TIn Then<TIn>(this TIn target, Action<TIn> continuation)
        {
            continuation(target);
            return target;
        }

        public static TOut Then<TIn, TOut>(this TIn target, Func<TIn, TOut> continuation)
        {
            return continuation(target);
        }
    }
}