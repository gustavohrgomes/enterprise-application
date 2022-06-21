using NSE.Identidade.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
{
    services.AddIdentityconfiguration(builder.Configuration);

    services.AddControllers();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}