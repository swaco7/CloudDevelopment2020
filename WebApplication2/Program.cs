using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication2 {
	public class Program {
		public static void Main(string[] args) {
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
			return WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, config) => {
					if (context.HostingEnvironment.IsProduction()) {
						var azureServiceTokenProvider = new AzureServiceTokenProvider();
						var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

						config.AddAzureKeyVault("https://mykeyvaultrs.vault.azure.net/", keyVaultClient, new DefaultKeyVaultSecretManager());
					}
				})
				.UseStartup<Startup>();
		}
	}
}
