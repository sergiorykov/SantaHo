namespace SantaHo.Core.Configuration
{
    public interface ISettingsMigrationRegistrar
    {
        void Register<TSettings>(TSettings defaultValue) where TSettings : class;

        void Register<TOriginalSettings, TCurrentSettings>(
            SettingsConverter<TOriginalSettings, TCurrentSettings> converter)
            where TOriginalSettings : class
            where TCurrentSettings : class;
    }
}