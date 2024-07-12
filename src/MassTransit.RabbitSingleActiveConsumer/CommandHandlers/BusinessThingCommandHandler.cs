using System;
using System.Threading.Tasks;
using MassTransit.RabbitSingleActiveConsumer.Commands;

namespace MassTransit.RabbitSingleActiveConsumer.CommandHandlers
{
    // TODO: actually ToDon't, pretty sure this isn't sufficient because we need this as the "receiver" end, not just at the per-consumer level...
    //public class BusinessThingCommandHandlerDefinition : ConsumerDefinition<BusinessThingCommandHandler>
    //{
    //    public BusinessThingCommandHandlerDefinition()
    //    {
    //        // override the default endpoint name, for whatever reason
    //        //EndpointName = "ha-submit-order";
    //        // limit the number of messages consumed concurrently
    //        // this applies to the consumer only, not the endpoint
    //        ConcurrentMessageLimit = 1;
    //    }
    //    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<BusinessThingCommandHandler> consumerConfigurator)
    //    {
    //        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
    //        endpointConfigurator.UseInMemoryOutbox();
    //    }
    //}

    public class BusinessThingCommandHandler : IConsumer<BusinessThing>
    {
        public async Task Consume(ConsumeContext<BusinessThing> context)
        {
            var success = false;
            try
            {
                // retrieve data from store by thingId

                // thing.Do();

                if (DateTimeOffset.UtcNow.Ticks % 20 == 0)
                {
                    throw new InvalidOperationException();
                }

                Console.WriteLine($"ThingId: {context.Message.ThingId} success.");
                success = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ThingId: {context.Message.ThingId} error: {exception.Message}.");
                // thing.DoError();
            }

            await context.RespondAsync(new
            {
                context.Message.CommandId,
                context.Message.ThingId,
                Success = success,
            });
        }
    }
}