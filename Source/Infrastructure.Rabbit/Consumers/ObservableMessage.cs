using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FluffyRabbit.Consumers
{
    internal sealed class ObservableMessage<TMessage> : IObservableMessage<TMessage>
    {
        private readonly IModel _channel;
        private readonly BasicDeliverEventArgs _deliverEventArgs;

        public ObservableMessage(BasicDeliverEventArgs deliverEventArgs, IModel channel)
        {
            _deliverEventArgs = deliverEventArgs;
            _channel = channel;

            var body = deliverEventArgs.Body;
            var message = Encoding.UTF8.GetString(body);
            Message = JsonConvert.DeserializeObject<TMessage>(message);
        }

        public TMessage Message { get; private set; }

        public void Completed()
        {
            _channel.BasicAck(_deliverEventArgs.DeliveryTag, false);
        }

        public void Failed()
        {
            _channel.BasicReject(_deliverEventArgs.DeliveryTag, true);
        }
    }
}
