using BookCatalog.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace BookCatalog;

public static class MvcExtenisons
{
    public static IServiceCollection AddClientMvcServices(this IServiceCollection services, IHostBuilder hostBuilder,IConfiguration configuration)
    {
        ConfigureSerilog(hostBuilder);
        ConfigureDatabase(services, configuration);

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void ConfigureSerilog(IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Min warning loglarını alır, sadece yaptığım info loglarını görmek, karmaşılığı azaltmak için.
                    .WriteTo.Console() // Consoleda da görmek için yaptım.
                    .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        hostBuilder.UseSerilog();
    }
}
