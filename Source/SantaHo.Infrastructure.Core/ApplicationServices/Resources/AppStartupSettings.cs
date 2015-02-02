using System;
using System.Collections.Generic;
using System.Configuration;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.Configuration;
using SantaHo.Core.Extensions;

namespace SantaHo.Infrastructure.Core.ApplicationServices.Resources
{
    public class AppStartupSettings : IStartupSettings
    {
        public TValue GetValue<TValue>(string key)
        {
            string value = GetRawValueBy(key);
            if (typeof (Enum).IsAssignableFrom(typeof (TValue)))
                return (TValue) Enum.Parse(typeof (TValue), value);
            return (TValue) Convert.ChangeType(value, typeof (TValue));
        }

        private static string GetRawValueBy(string key)
        {
            string value = ConfigurationManager.AppSettings["startup:{0}".FormatWith(key)];
            if (value == null)
            {
                throw new KeyNotFoundException("Key " + key + " not found");
            }
            return value;
        }
    }
}