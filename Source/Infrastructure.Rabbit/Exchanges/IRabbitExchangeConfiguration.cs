namespace FluffyRabbit.Exchanges
{
    public interface IRabbitExchangeConfiguration
    {
        string Name { get; set; }
        RabbitExchangeType Type { get; set; }
    }
}