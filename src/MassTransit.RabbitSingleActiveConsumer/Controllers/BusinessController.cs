using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.RabbitSingleActiveConsumer.Controllers;

[Route("api/business/business/business/numbers"), ApiController]
public class BusinessController : Controller
{
    private readonly IBus _messageBus;
    //private readonly IPublishEndpointProvider _publishEndpointProvider;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    //private readonly ISendEndpoint _sendEndpoint;

    public BusinessController(
        IBus messageBus,
        //IPublishEndpointProvider publishEndpointProvider,
        IPublishEndpoint publishEndpoint,
        ISendEndpointProvider sendEndpointProvider//,
        //ISendEndpoint sendEndpoint
    )
    {
        _messageBus = messageBus;
        //_publishEndpointProvider = publishEndpointProvider;
        _publishEndpoint = publishEndpoint;
        _sendEndpointProvider = sendEndpointProvider;
        //_sendEndpoint = sendEndpoint;
    }

    /// <remarks>
    /// utilizes a protected resource
    /// </remarks>
    [HttpPost("thing")]
    public async Task<IActionResult> DoBusinessThing(Guid thingId, int batches = 1)
    {
        var commandId = Guid.NewGuid();

        var publishTasks = new List<Task>();
        Console.WriteLine("Start creating tasks.");
        for (var batch = 0; batch < batches; batch++)
        {
            // persist a lot of data to a store using thingId
            var command = new BusinessThing(commandId, thingId, batch);
            publishTasks.Add(_messageBus.Publish(command));
        }
        Console.WriteLine("Stop creating tasks.");
        await Task.WhenAll(publishTasks);
        Console.WriteLine("Tasks complete.");

        // Various Attempts:
        //await _messageBus.Send(command, pipe: new MessageSendPipe<BusinessThing>(new AsyncPipeContextPipe<SendContext<BusinessThing>>()));
        //await _sendEndpointProvider.Send(command, new AsyncPipeContextPipe<SendContext<BusinessThing>>());
        //context => {
        //    context.Headers.Set("x-single-active-consumer", true);
        //}

        return Ok(new { CommandId = commandId, ThingId = thingId});
    }

    /// <remarks>
    /// no gate keeping, process everything as fast as possible on all known consumers
    /// </remarks>
    [HttpPost("doodad")]
    public async Task<IActionResult> ProcessBusinessDoodad(Guid doodadId, int batches = 1)
    {
        var commandId = Guid.NewGuid();

        var publishTasks = new List<Task>();
        Console.WriteLine("Start creating tasks.");
        for (var batch = 0; batch < batches; batch++)
        {
            // persist a lot of data to a store using doodadId
            var command = new BusinessDoodad(commandId, doodadId, batch);
            publishTasks.Add(_messageBus.Publish(command));
        }
        Console.WriteLine("Stop creating tasks.");
        await Task.WhenAll(publishTasks);
        Console.WriteLine("Tasks complete.");
       
        return Ok(new { CommandId = commandId, DoodadId = doodadId});
    }
}