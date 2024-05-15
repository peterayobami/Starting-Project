namespace Starting_Project
{
    public class ApplicationLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider provider;

        public ApplicationLoggerProvider(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ApplicationLogger(provider, categoryName);
        }

        public void Dispose()
        {
            
        }
    }
}