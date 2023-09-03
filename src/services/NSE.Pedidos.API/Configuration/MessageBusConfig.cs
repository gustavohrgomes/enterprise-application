using MassTransit;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Pedidos.API.Services;

namespace NSE.Pedidos.API.Configuration;

public static class MessageBusConfig
{
    public static IServiceCollection AddRabbitMQMessagingConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("RabbitMQ:Host");
        var virtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost");
        var username = configuration.GetValue<string>("RabbitMQ:Username");
        var password = configuration.GetValue<string>("RabbitMQ:Password");

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumers(typeof(Program).Assembly);

            configurator.AddRequestClient<PedidoIniciadoIntegrationEvent>();
            
            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.UsingRabbitMq((ctx, rabbit) =>
            {
                rabbit.Host(host, virtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(username);
                    hostConfigurator.Password(password);
                });

                rabbit.ReceiveEndpoint("pedido-cancelado", config =>
                {
                    config.ConfigureConsumer<PedidoCanceladoConsumer>(ctx);
                });
                
                rabbit.ReceiveEndpoint("pedido-pago", config =>
                {
                    config.ConfigureConsumer<PedidoPagoConsumer>(ctx);
                });

                rabbit.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
