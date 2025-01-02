using MassTransit;
using NSE.Carrinho.API.Services;

namespace NSE.Carrinho.API.Configuration;

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
            configurator.AddConsumer<PedidoRealizadoConsumer>();
            
            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.UsingRabbitMq((ctx, rabbit) =>
            {
                rabbit.Host(host, virtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(username);
                    hostConfigurator.Password(password);
                });
                
                rabbit.ReceiveEndpoint("pedido-realizado", config =>
                {
                    config.ConfigureConsumer<PedidoRealizadoConsumer>(ctx);
                });

                rabbit.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}