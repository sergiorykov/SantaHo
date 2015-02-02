using System;
using System.Reflection;

namespace SantaHo.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatWith(this string format, params object[] values)
        {
            return string.Format(format, values);
        }

        public static void Throw<TException>(this string message) where TException : Exception
        {
            ConstructorInfo constructorInfo = typeof (TException).GetConstructor(new[] {typeof (string)});
            if (constructorInfo != null)
            {
                throw (TException) constructorInfo.Invoke(new object[] {message});
            }

            throw new InvalidOperationException("Type {0} must have ctor(message)".FormatWith(typeof (TException).FullName));
        }

        public static void Throw<TException>(this string format, params object[] values) where TException : Exception
        {
            format.FormatWith(values).Throw<TException>();
        }
    }
}