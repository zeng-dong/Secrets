using Microsoft.Extensions.Configuration;
using Serilog;

namespace InConsoleApp
{
    public class ConfigInterpreter
    {
        private readonly IConfiguration config;
        public Settings Settings { get; }

        public ConfigInterpreter(IConfiguration config)
        {
            this.config = config;
            this.Settings = this.config.GetSection("secrets").Get<Settings>();
        }
        public void DescribeSettings()
        {
            Log.Information($"ConfigInterpreter -> settings -> Email= {Settings.Address.Email }");
            Log.Information($"ConfigInterpreter -> settings -> Shipping=  { Settings.Address.Shipping}");
            Log.Information($"ConfigInterpreter -> settings -> Password:  ({ Settings.Password})");
            Log.Information($"ConfigInterpreter -> settings -> ConnectionString:  ({ Settings.ConnectionStringOne})");
        }
    }
}
