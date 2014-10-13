namespace SantaHo.Domain.Configuration
{
    public interface ISettingsRepository
    {
        TValue Get<TValue>(string key);
        void Set<TValue>(string key, TValue value);
    }
}