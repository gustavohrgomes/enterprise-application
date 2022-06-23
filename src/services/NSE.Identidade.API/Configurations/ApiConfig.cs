namespace NSE.Identidade.API.Configurations;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers();

        return services;
    }

    public static WebApplication UseApiConfiguration(this WebApplication app)
    {
        app.UseRouting();

        app.UseIdentityConfiguration();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
