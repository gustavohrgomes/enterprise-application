using MassTransit;
using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pagamentos.API.Services;

namespace NSE.Pagamentos.API.Configuration;

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
            
            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.UsingRabbitMq((ctx, rabbit) =>
            {
                rabbit.Host(host, virtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(username);
                    hostConfigurator.Password(password);
                });
                
                rabbit.ReceiveEndpoint("pedido-inciado", config =>
                {
                    config.ConfigureConsumer<PedidoIniciadoConsumer>(ctx);
                });

                rabbit.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
