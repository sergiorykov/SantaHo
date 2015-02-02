namespace SantaHo.Core.ApplicationServices
{
    public interface IStartupSettings
    {
        TValue GetValue<TValue>(string key);
    }
}