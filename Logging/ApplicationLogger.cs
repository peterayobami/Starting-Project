namespace Starting_Project
{
    public class ApplicationLogger : ILogger
    {
        private readonly IServiceProvider provider;

        private readonly string categoryName;

        public ApplicationLogger(IServiceProvider provider, string categoryName)
        {
            this.provider = provider;
            this.categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Set the log details
            // Set default partition id for system logs
            var logEntry = new SystemLogDataModel
            {
                ProgramId = "system-logs",
                LogLevel = logLevel.ToString(),
                Message = formatter(state, exception)
            };

            // Create the log
            context.SystemLogs.Add(logEntry);

            // Save changes
            context.SaveChanges();
        }
    }
}