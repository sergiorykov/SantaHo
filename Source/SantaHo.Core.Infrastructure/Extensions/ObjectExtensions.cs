using System;
using NLog;

namespace SantaHo.Core.Infrastructure.Extensions
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