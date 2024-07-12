using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.RabbitSingleActiveConsumer.Controllers
{
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
        public async Task<IActionResult> DoBusinessThing(Guid thingId)
        {
            // persist a lot of data to a store using thingId

            var command = new BusinessThing(commandId: Guid.NewGuid(), thingId);

            await _messageBus.Publish(command);

            // Various Attempts:
            //await _messageBus.Send(command, pipe: new MessageSendPipe<BusinessThing>(new AsyncPipeContextPipe<SendContext<BusinessThing>>()));
            //await _sendEndpointProvider.Send(command, new AsyncPipeContextPipe<SendContext<BusinessThing>>());
            //context => {
            //    context.Headers.Set("x-single-active-consumer", true);
            //}

            return Ok(new { ThingId = thingId});
        }

        /// <remarks>
        /// no gate keeping, process everything as fast as possible on all known consumers
        /// </remarks>
        [HttpPost("doodad")]
        public async Task<IActionResult> ProcessBusinessDoodad(Guid doodadId)
        {
            // persist a lot of data to a store using doodadId

            var command = new BusinessDoodad(commandId: Guid.NewGuid(), doodadId);

            await _messageBus.Publish(command);

            return Ok(new { DoodadId = doodadId});
        }
    }
}