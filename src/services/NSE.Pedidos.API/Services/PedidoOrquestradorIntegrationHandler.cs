﻿using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pedidos.API.Application.Queries;

namespace NSE.Pedidos.API.Services;

public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
{
    private ILogger<PedidoOrquestradorIntegrationHandler> _logger;
    private IServiceProvider _serviceProvider;
    private Timer _timer;

    public PedidoOrquestradorIntegrationHandler(ILogger<PedidoOrquestradorIntegrationHandler> logger, 
                                                IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedidos iniciado.");

        _timer = new Timer(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(90));

        return Task.CompletedTask;
    }

    private async void ProcessarPedidos(object state)
    {
        _logger.LogInformation("Iniciando processamento de pedidos.");
        using var scope = _serviceProvider.CreateScope();
        
        var pedidoQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();
        var pedido = await pedidoQueries.ObterPedidosAutorizados();

        if (pedido is null) return;

        var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

        var pedidoAutorizado = new PedidoAutorizadoIntegrationEvent(
            pedido.ClienteId,
            pedido.Id,
            pedido.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));

        await bus.PublishAsync(pedidoAutorizado);

        _logger.LogInformation($"Pedido ID: {pedido.Id} foi encaminhado para baixa no estoque.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedidos finalizado.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    #region Disposable

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _timer.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
