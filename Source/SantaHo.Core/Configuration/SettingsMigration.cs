using System;
using System.Collections.Generic;
using System.Configuration;
using SantaHo.Core.Extensions;

namespace SantaHo.Core.Configuration
{
    public class SettingsMigration : ISettingsMigrationRegistrar
    {
        private readonly List<Action> _converters = new List<Action>();
        private readonly List<Action> _defaultSettings = new List<Action>();
        private readonly ISettingsRepository _settingsRepository;

        public SettingsMigration(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public void Register<TSettings>(TSettings defaultValue) where TSettings : class
        {
            _defaultSettings.Add(() => Apply(defaultValue));
        }

        public void Register<TOriginalSettings, TCurrentSettings>(
            SettingsConverter<TOriginalSettings, TCurrentSettings> converter)
            where TOriginalSettings : class
            where TCurrentSettings : class
        {
            _converters.Add(() => Apply(converter));
        }

        public void Apply()
        {
            _defaultSettings.ForEach(x => x());
            _converters.ForEach(x => x());
        }

        private void Apply<TSettings>(TSettings value) where TSettings : class
        {
            var existingValue = _settingsRepository.Get<TSettings>();
            if (existingValue == null)
            {
                _settingsRepository.Set(value);
            }
        }

        private void Apply<TOriginalSettings, TCurrentSettings>(
            SettingsConverter<TOriginalSettings, TCurrentSettings> converter) where TOriginalSettings : class
            where TCurrentSettings : class
        {
            var originalValue = _settingsRepository.Get<TOriginalSettings>();
            if (originalValue == null)
            {
                "Settings not found for {0}:"
                    .FormatWith(typeof (TOriginalSettings).FullName)
                    .Throw<SettingsPropertyNotFoundException>();
            }

            TCurrentSettings currentValue = converter.Convert(originalValue);
            _settingsRepository.Set(currentValue);
        }
    }
}