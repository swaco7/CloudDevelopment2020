using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1 {
    public class Startup {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services) {
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			app.UseDeveloperExceptionPage();

			app.Run(async (context) => {
				string vaultAddress = "https://mykeyvaultrs.vault.azure.net/";
				string secretName = "MySecretRS";

				var tokenProvider = new AzureServiceTokenProvider();
				var kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
				var secret = await kvClient.GetSecretAsync(vaultAddress, secretName);

				await context.Response.WriteAsync(secret.Value);
			});
		}
	}
}
