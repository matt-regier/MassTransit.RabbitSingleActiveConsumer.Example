using System;

namespace MassTransit.RabbitSingleActiveConsumer
{
    public class BusinessThing
    {
        public Guid CommandId { get; }
        public Guid ThingId { get; }

        public BusinessThing(Guid commandId, Guid thingId)
        {
            CommandId = commandId;
            ThingId = thingId;
        }
    }
}