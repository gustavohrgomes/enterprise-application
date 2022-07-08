using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Configuration;

public static class WebAppConfig
{
    public static void AddWebAppConfiguration(this IServiceCollection services)
    {
        services.AddControllersWithViews();
    }

    public static void UseWebAppConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/erro/500");
            app.UseStatusCodePagesWithRedirects("/erro/{0}");
            app.UseHsts();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityConfiguration();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Catalogo}/{action=Index}/{id?}");
        });
    }
}
