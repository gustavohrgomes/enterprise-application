using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using NSE.WebApp.MVC.Extensions;
using System.Globalization;

namespace NSE.WebApp.MVC.Configuration;

public static class WebAppConfig
{
    public static void AddWebAppConfiguration(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.Configure<ForwardedHeadersOptions>(options => 
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
    }

    public static void UseWebAppConfiguration(this WebApplication app)
    {
        app.UseForwardedHeaders();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/erro/500");
            app.UseStatusCodePagesWithRedirects("/erro/{0}");
        }

        // Under certain scenarios, e.g minikube / linux environment / behind load balancer
        // https redirection could lead dev's to over complicated configuration for testing purpouses
        // In production is a good practice to keep it true
        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
        {
            app.UseHttpsRedirection();
            app.UseHsts();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityConfiguration();

        var supportedCultures = new[] { new CultureInfo("pt-BR") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Catalogo}/{action=Index}/{id?}");
        });
    }
}
