namespace SantaHo.Domain.Configuration
{
    public abstract class SettingsConverter<TOriginalSettings, TCurrentSettings>
    {
        public bool PreserveOriginal { get; protected set; }

        public TCurrentSettings Convert(TOriginalSettings originalSettings)
        {
            return ConvertCore(originalSettings);
        }

        protected abstract TCurrentSettings ConvertCore(TOriginalSettings originalSettings);
    }
}