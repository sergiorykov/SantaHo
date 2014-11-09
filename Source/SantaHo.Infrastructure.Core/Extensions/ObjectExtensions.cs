using System;
using System.Collections.Generic;
using NLog;

namespace SantaHo.Infrastructure.Core.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void IgnoreFailureWhen<TValue>(this TValue value, Action<TValue> action)
        {
            try
            {
                action(value);
            }
            catch (Exception e)
            {
                Logger.Warn(e);
            }
        }
        
        public static void IgnoreFailuresWhen<TValue>(this IEnumerable<TValue> values, Action<TValue> action)
        {
            foreach (TValue value in values)
            {
                value.IgnoreFailureWhen(action);
            }
        }

        public static bool FailIfNot<TValue>(this TValue value, Action<TValue> action)
        {
            try
            {
                action(value);
                return true;
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                Logger.Error("Action failed");
                return false;
            }
        }
    }
}