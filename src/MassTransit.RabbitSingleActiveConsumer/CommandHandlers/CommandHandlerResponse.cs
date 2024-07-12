using System;

namespace MassTransit.RabbitSingleActiveConsumer;

public class CommandHandlerResponse
{
    public CommandHandlerResponse(Guid commandId, bool success)
    {
        CommandId = commandId;
        Success = success;
    }

    public Guid CommandId { get; set; }
    public bool Success { get; set; }
}