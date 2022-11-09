using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Data.Mappings;

public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.HasKey(t => t.Id);

        // 1 : N => Pagamento : Transacao
        builder.HasOne(t => t.Pagamento)
            .WithMany(t => t.Transacoes);

        builder.ToTable("Transacoes");
    }
}
