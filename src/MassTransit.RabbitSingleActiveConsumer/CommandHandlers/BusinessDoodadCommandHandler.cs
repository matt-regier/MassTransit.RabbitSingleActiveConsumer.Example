using System;
using System.Threading.Tasks;

namespace MassTransit.RabbitSingleActiveConsumer
{
    public class BusinessDoodadCommandHandler : IConsumer<BusinessDoodad>
    {
        public async Task Consume(ConsumeContext<BusinessDoodad> context)
        {
            var success = false;
            try
            {
                // retrieve data from store by doodadId

                // doodad.Process();

                if (DateTimeOffset.UtcNow.Ticks % 20 == 0)
                {
                    throw new InvalidOperationException();
                }

                Console.WriteLine($"DoodadId: {context.Message.DoodadId} success.");
                success = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"DoodadId: {context.Message.DoodadId} error: {exception.Message}.");
                // doodad.ErrorProcess();
            }

            await context.RespondAsync(new
            {
                context.Message.CommandId,
                context.Message.DoodadId,
                Success = success,
            });
        }
    }
}