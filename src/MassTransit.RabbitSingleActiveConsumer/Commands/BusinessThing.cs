using System;

namespace MassTransit.RabbitSingleActiveConsumer;

public class BusinessThing
{
    public BusinessThing(Guid commandId, Guid thingId, int batchId)
    {
        CommandId = commandId;
        ThingId = thingId;
        BatchId = batchId;
    }

    public Guid CommandId { get; }
    public Guid ThingId { get; }
    public int BatchId { get; }
}