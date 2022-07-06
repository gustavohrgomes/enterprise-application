using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Data;
using NSE.Catalogo.API.Data.Repositories;
using NSE.Catalogo.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<CatalogContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
