using System;

namespace MassTransit.RabbitSingleActiveConsumer;

public class BusinessDoodad
{
    public BusinessDoodad(Guid commandId, Guid doodadId, int batchId)
    {
        CommandId = commandId;
        DoodadId = doodadId;
        BatchId = batchId;
    }

    public Guid CommandId { get; }
    public Guid DoodadId { get; }
    public int BatchId { get; }
}