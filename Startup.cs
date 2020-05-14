using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PopHistoryFunction.EntityFramework;

#if DEBUG
using System.Reflection;
#else
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
#endif

[assembly: FunctionsStartup(typeof(PopHistoryFunction.Startup))]
namespace PopHistoryFunction
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();

#if DEBUG
            // Local Development
            // CMD Setup: dotnet user-secrets set <key> <value>
            configurationBuilder.AddUserSecrets(Assembly.GetExecutingAssembly(), false);
#else
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));

            configurationBuilder.AddAzureKeyVault(
                "https://pophistory.vault.azure.net/",
                keyVaultClient,
                new DefaultKeyVaultSecretManager());
#endif

            var config = configurationBuilder.Build();
            builder.Services.AddSingleton<IConfiguration>(config);

            string connectionString = config["ConnectionStrings:SqlServer"];

            builder.Services.AddDbContext<PopHistoryFunctionContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
        }
    }
}
