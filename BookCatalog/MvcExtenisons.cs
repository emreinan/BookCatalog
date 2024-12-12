using BookCatalog.Data.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace BookCatalog;

public static class MvcExtenisons
{
    public static void ConfigureSerilog(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Min warning loglarını alır, sadece yaptığım info loglarını görmek, karmaşılığı azaltmak için.
            .WriteTo.Console() // Consoleda da görmek için yaptım.
            .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        hostBuilder.UseSerilog();
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}
