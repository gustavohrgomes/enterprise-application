﻿using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Model;

namespace NSE.Carrinho.API.Data;

public sealed class CarrinhoContext : DbContext
{
    public CarrinhoContext(DbContextOptions<CarrinhoContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<CarrinhoItem> CarrinhoItens { get; set; }
    public DbSet<CarrinhoCliente> CarrinhoCliente { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(250);

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();

        modelBuilder.Entity<CarrinhoCliente>()
            .HasIndex(c => c.ClienteId)
            .HasDatabaseName("IDX_Cliente");

        modelBuilder.Entity<CarrinhoCliente>()
                .Ignore(c => c.Voucher)
                .OwnsOne(c => c.Voucher, v =>
                {
                    v.Property(vc => vc.Codigo)
                        .HasColumnName("VoucherCodigo")
                        .HasColumnType("varchar(50)");

                    v.Property(vc => vc.TipoDesconto)
                        .HasColumnName("TipoDesconto");

                    v.Property(vc => vc.Percentual)
                        .HasColumnName("Percentual");

                    v.Property(vc => vc.ValorDesconto)
                        .HasColumnName("ValorDesconto");
                });

        modelBuilder.Entity<CarrinhoCliente>()
            .HasMany(c => c.Itens)
            .WithOne(i => i.CarrinhoCliente)
            .HasForeignKey(c => c.CarrinhoId);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
    }
}
