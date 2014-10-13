namespace SantaHo.Domain.Configuration
{
    public interface ISupportSettingsMigration
    {
        void PrepareSettings(ISettingsMigrationRegistrar registrar);
    }
}