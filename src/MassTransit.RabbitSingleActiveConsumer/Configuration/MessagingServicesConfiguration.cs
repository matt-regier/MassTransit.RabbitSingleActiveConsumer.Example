using System;
using System.Net.Sockets;
using MassTransit;
using MassTransit.Configuration;
using MassTransit.RabbitSingleActiveConsumer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class MessagingServicesConfiguration
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        return services
                .AddMassTransit(opt => SetupInternalBus(opt, config, env))
            ;
    }

    private static void SetupInternalBus(IBusRegistrationConfigurator opt, IConfiguration config, IWebHostEnvironment env)
    {
        // TODO: maybe this is the problem? we're adding all the consumers before we get to the ReceiveEndpoint where we wire in BusinessThingCommandHandler?
        opt.AddConsumersFromNamespaceContaining<BusinessDoodadCommandHandler>();

        opt.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(
                new Uri("rabbitmq://localhost:5672"), 
                h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

            //var endpointNameFormatter = new DefaultEndpointNameFormatter(false);
            //var endpointNameFormatter = new DefaultEndpointNameFormatter(null);

            var autoWireAllConsumers = true;
            // TODO: which of these works?
            if (false)
            {
                autoWireAllConsumers = false;
                var endpointSettings = new EndpointSettings<IEndpointDefinition<BusinessThingCommandHandler>>
                {
                    //Name = null,
                    IsTemporary = false,
                    PrefetchCount = 1,
                    ConcurrentMessageLimit = 1,
                    //ConfigureConsumeTopology = false,
                    //InstanceId = null
                };
                var consumerEndpointDefinition = new ConsumerEndpointDefinition<BusinessThingCommandHandler>(endpointSettings);
                cfg.ReceiveEndpoint(
                    consumerEndpointDefinition,
                    //endpointNameFormatter,
                    configureEndpoint: e =>
                    {
                        e.Durable = true;
                        e.PrefetchCount = 1; // only applies to this endpoint
                        e.ConcurrentMessageLimit = 1; // only applies to this endpoint
                        e.SetQueueArgument("x-single-active-consumer", true);
                        e.ConfigureConsumer<BusinessThingCommandHandler>(context);
                    });
            }
            else if (false)
            {
                autoWireAllConsumers = false;
                cfg.ReceiveEndpoint(
                    nameof(BusinessThing),
                    e =>
                    {
                        e.SingleActiveConsumer = true;
                        e.Durable = true;
                        e.PrefetchCount = 1;
                        e.ConcurrentMessageLimit = 1; // only applies to this endpoint
                        //e.SetQueueArgument("x-single-active-consumer", true);
                        //e.SetQueueArgument("x-queue-type", "classic");
                        //e.SetQueueArgument("durable", true);
                        e.Consumer<BusinessThingCommandHandler>(context);
                        e.ConfigureConsumer<BusinessThingCommandHandler>(context);
                    }
                );
            }

            if (autoWireAllConsumers)
            {
                context.ConfigureEndpoints(cfg, null/*endpointNameFormatter*/);
            }
            else
            {
                // Order Matters: Manually configured receive endpoints should be configured before calling ConfigureEndpoints.
                context.ConfigureEndpoints(
                    cfg,
                    null/*endpointNameFormatter*/,
                    _ => _.Exclude<BusinessThingCommandHandler>()
                );
            }

            // TODO: custom extension method to look into.
            //cfg.MapEndpointConventions(new[]
            //{
            //    typeof(BusinessDoodad),
            //    typeof(BusinessThing)
            //});
        });
    }
}