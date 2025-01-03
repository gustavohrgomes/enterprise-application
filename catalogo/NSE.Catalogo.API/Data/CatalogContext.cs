﻿using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Messages;

namespace NSE.Catalogo.API.Data;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(250);

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Ignore<ValidationResult>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CommitAsync() => await base.SaveChangesAsync() > 0;
}
