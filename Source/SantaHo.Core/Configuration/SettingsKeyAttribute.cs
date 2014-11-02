using System;

namespace SantaHo.Core.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SettingsKeyAttribute : Attribute
    {
        public SettingsKeyAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            Key = key;
        }

        public string Key { get; private set; }
    }
}