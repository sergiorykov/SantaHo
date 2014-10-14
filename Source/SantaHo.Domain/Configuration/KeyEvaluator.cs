using System;
using System.Linq;
using SantaHo.Core.Extensions;
using ServiceStack;

namespace SantaHo.Domain.Configuration
{
    public class KeyEvaluator
    {
        public string GetKey<TValue>()
        {
            return GetKeyCore<TValue>();
        }

        protected virtual string GetKeyCore<TValue>()
        {
            string typeKey = GetTypeKey<TValue>();
            return "{0}:working".F(typeKey);
        }

        private static string GetTypeKey<TValue>()
        {
            Type valueType = typeof (TValue);
            SettingsKeyAttribute settingsKey = valueType.AllAttributes<SettingsKeyAttribute>().FirstOrDefault();
            if (settingsKey != null)
            {
                return settingsKey.Key;
            }

            return valueType.FullName;
        }
    }
}