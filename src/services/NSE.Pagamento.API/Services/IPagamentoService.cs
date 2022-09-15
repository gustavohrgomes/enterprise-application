using NSE.Core.Messages.IntegrationEvents;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services;

public interface IPagamentoService
{
    Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
}
