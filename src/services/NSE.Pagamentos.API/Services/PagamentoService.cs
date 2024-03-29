﻿using FluentValidation.Results;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Pagamentos.API.Facade;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services;

public class PagamentoService : IPagamentoService
{
    private readonly IPagamentoFacade _pagamentoFacade;
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PagamentoService(IPagamentoFacade pagamentoFacade, IPagamentoRepository pagamentoRepository, IUnitOfWork unitOfWork)
    {
        _pagamentoFacade = pagamentoFacade ?? throw new ArgumentNullException(nameof(pagamentoFacade));
        _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento)
    {
        var transacao = await _pagamentoFacade.AutorizarPagamento(pagamento);
        var validationResult = new ValidationResult();

        if (transacao.Status != StatusTransacao.Autorizado)
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento Recusado, entre em contato com a sua operadora de cartão"));

            return new ResponseMessage(validationResult);
        }

        pagamento.AdicionarTransacao(transacao);
        _pagamentoRepository.AdicionarPagamento(pagamento);

        if (!await _unitOfWork.CommitAsync())
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar o pagamento"));

            // TODO: Comunicar com o gateway para realizar o estorno

            return new ResponseMessage(validationResult);
        }

        return new ResponseMessage(validationResult);
    }

    public async Task<ResponseMessage> CapturarPagamento(Guid pedidoId)
    {
        var transacoes = await _pagamentoRepository.ObterTransacoesPorPedidoId(pedidoId);
        var transacaoAutorizada = transacoes?.FirstOrDefault(t => t.Status == StatusTransacao.Autorizado);
        var validationResult = new ValidationResult();

        if (transacaoAutorizada is null) throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

        var transacao = await _pagamentoFacade.CapturarPagamento(transacaoAutorizada);

        if (transacao.Status is not StatusTransacao.Pago)
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", $"Não foi possível capturar o pagamento do pedido {pedidoId}"));

            return new ResponseMessage(validationResult);
        }

        transacao.PagamentoId = transacaoAutorizada.PagamentoId;
        _pagamentoRepository.AdicionarTransacao(transacao);

        if (!await _unitOfWork.CommitAsync())
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", $"Não foi possível persistir a captura do pagamento do pedido {pedidoId}"));

            return new ResponseMessage(validationResult);
        }

        return new ResponseMessage(validationResult);
    }

    public async Task<ResponseMessage> CancelarPagamento(Guid pedidoId)
    {
        var transacoes = await _pagamentoRepository.ObterTransacoesPorPedidoId(pedidoId);
        var transacaoAutorizada = transacoes?.FirstOrDefault(t => t.Status == StatusTransacao.Autorizado);
        var validationResult = new ValidationResult();

        if (transacaoAutorizada is null) throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

        var transacao = await _pagamentoFacade.CancelarAutorizacao(transacaoAutorizada);

        if (transacao.Status is not StatusTransacao.Cancelado)
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", $"Não foi possível cancelar o pagamento do pedido {pedidoId}"));

            return new ResponseMessage(validationResult);
        }

        transacao.PagamentoId = transacaoAutorizada.PagamentoId;
        _pagamentoRepository.AdicionarTransacao(transacao);

        if (!await _unitOfWork.CommitAsync())
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", $"Não foi possível persistir o cancelamento do pagamento do pedido {pedidoId}"));

            return new ResponseMessage(validationResult);
        }

        return new ResponseMessage(validationResult);
    }
}
