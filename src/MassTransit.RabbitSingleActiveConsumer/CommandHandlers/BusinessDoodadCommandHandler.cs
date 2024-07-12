using System;
using System.Threading.Tasks;

namespace MassTransit.RabbitSingleActiveConsumer;

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

            Console.WriteLine($"DoodadId: {context.Message.DoodadId} BatchId: {context.Message.BatchId} success.");
            success = true;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"DoodadId: {context.Message.DoodadId} BatchId: {context.Message.BatchId} error: {exception.Message}.");
            // doodad.ErrorProcess();
        }

        await context.RespondAsync(new CommandHandlerResponse(context.Message.CommandId, success));
    }
}