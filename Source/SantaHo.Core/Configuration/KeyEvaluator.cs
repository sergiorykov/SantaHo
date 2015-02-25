using System;
using System.Linq;
using System.Reflection;
using SantaHo.Core.Extensions;

namespace SantaHo.Core.Configuration
{
    public class KeyEvaluator
    {
        public const string DefaultGroup = "working";

        public KeyEvaluator() : this(DefaultGroup)
        {
        }

        public KeyEvaluator(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException("groupName");
            }

            KeyPrefix = "settings:{0}:".FormatWith(groupName);
        }

        protected string KeyPrefix { get; set; }

        public string GetKey<TValue>()
        {
            return GetKeyCore<TValue>();
        }

        protected virtual string GetKeyCore<TValue>()
        {
            string typeKey = GetTypeKey<TValue>();
            return "{0}:{1}".FormatWith(KeyPrefix, typeKey);
        }

        private static string GetTypeKey<TValue>()
        {
            Type valueType = typeof(TValue);
            SettingsKeyAttribute settingsKey = valueType.GetCustomAttributes<SettingsKeyAttribute>().FirstOrDefault();
            if (settingsKey != null)
            {
                return settingsKey.Key;
            }

            return valueType.FullName;
        }
    }
}
