using System;

namespace MassTransit.RabbitSingleActiveConsumer
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