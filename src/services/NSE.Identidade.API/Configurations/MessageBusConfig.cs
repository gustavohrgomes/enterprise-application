﻿using MassTransit;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Identidade.API.Configurations;

public static class MessageBusConfig
{
    public static IServiceCollection AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));

        return services;
    }

    public static IServiceCollection AddRabbitMQMessagingConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("RabbitMQ:Host");
        var virtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost");
        var username = configuration.GetValue<string>("RabbitMQ:Username");
        var password = configuration.GetValue<string>("RabbitMQ:Password");

        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();

            configurator.AddRequestClient<UsuarioRegistradoIntegrationEvent>();

            configurator.UsingRabbitMq((ctx, rabbit) =>
            {
                rabbit.Host(host, virtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(username);
                    hostConfigurator.Password(password);
                });

                rabbit.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}