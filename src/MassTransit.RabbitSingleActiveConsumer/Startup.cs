using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.RabbitSingleActiveConsumer;

public class Startup
{
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment CurrentEnvironment { get; }

    public Startup(IWebHostEnvironment env, IConfiguration config)
    {
        Configuration = config;
        CurrentEnvironment = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddHttpClient();
            
        services.AddMessageBus(Configuration, CurrentEnvironment);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}