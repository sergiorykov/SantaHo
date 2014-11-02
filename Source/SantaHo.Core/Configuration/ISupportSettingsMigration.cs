namespace SantaHo.Core.Configuration
{
    public interface ISupportSettingsMigration
    {
        void PrepareSettings(ISettingsMigrationRegistrar registrar);
    }
}