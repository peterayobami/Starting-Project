using Microsoft.EntityFrameworkCore;

namespace Starting_Project
{
    /// <summary>
    /// The extension methods for configuring the application's services
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Creates the application's main database, if it does not exist
        /// </summary>
        /// <param name="application">The <see cref="ApplicationBuilder"/> instance</param>
        /// <returns></returns>
        public async static Task<IApplicationBuilder> CreateApplicationDatabase(this IApplicationBuilder application)
        {
            // Create a service scope
            var scope = application.ApplicationServices.CreateScope();

            // Get the database context instance
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Create the database
            await context.Database.EnsureCreatedAsync();

            // Return application for further chaining
            return application;
        }

        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add database to DI
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseCosmos(configuration.GetConnectionString("Cosmos"), CosmosDatabases.Programmes);
            });

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Registers domain services to the DI container
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance</param>
        /// <returns>The <see cref="IServiceCollection"/> for further</returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            // Add domain services to DI

            services.AddScoped<ProgramService>()
                .AddScoped<ApplicationService>();

            // Return services
            return services;
        }

        /// <summary>
        /// Registers the singleton instance of the <see cref="ILoggerProvider"/>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationLogger(this IServiceCollection services)
        {
            // Add the singleton instance to DI
            services.AddSingleton<ILoggerProvider, ApplicationLoggerProvider>();

            // Return services for further chaining
            return services;
        }
    }
}