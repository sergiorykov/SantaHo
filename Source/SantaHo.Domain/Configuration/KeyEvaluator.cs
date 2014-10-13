using System.Linq;
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
            var valueType = typeof(TValue);
            var settingsKey = valueType.AllAttributes<SettingsKeyAttribute>().FirstOrDefault();
            if (settingsKey != null)
            {
                return settingsKey.Key;
            }

            return valueType.FullName;
        }
    }
}