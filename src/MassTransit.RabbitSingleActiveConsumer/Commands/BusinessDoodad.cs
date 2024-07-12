using System;

namespace MassTransit.RabbitSingleActiveConsumer.Commands
{
    public class BusinessDoodad
    {
        public Guid CommandId { get; }
        public Guid DoodadId { get; }

        public BusinessDoodad(Guid commandId, Guid doodadId)
        {
            CommandId = commandId;
            DoodadId = doodadId;
        }
    }
}