using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using SantaHo.Core.Configuration;

namespace SantaHo.Infrastructure.Core.Configuration
{
    public class AppSettingsRepository : ISettingsRepository
    {
        private readonly KeyEvaluator _keyEvaluator;

        public AppSettingsRepository(KeyEvaluator keyEvaluator)
        {
            if (keyEvaluator == null)
            {
                throw new ArgumentNullException("keyEvaluator");
            }

            _keyEvaluator = keyEvaluator;
        }

        public TValue Get<TValue>() where TValue : class
        {
            string key = _keyEvaluator.GetKey<TValue>();
            string value = ConfigurationManager.AppSettings[key];
            if (value == null)
            {
                throw new KeyNotFoundException("Key " + key + " not found");
            }

            return JsonConvert.DeserializeObject<TValue>(value);
        }

        public void Set<TValue>(TValue value) where TValue : class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string key = _keyEvaluator.GetKey<TValue>();
            string serializedValue = JsonConvert.SerializeObject(value);
            ConfigurationManager.AppSettings.Set(key, serializedValue);
        }
    }
}
