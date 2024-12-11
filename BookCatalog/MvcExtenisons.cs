using BookCatalog.Data.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace BookCatalog;

public static class MvcExtenisons
{
    public static void ConfigureSerilog(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        hostBuilder.UseSerilog();
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}
