using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication2 {
	public class Startup {
		public void ConfigureServices(IServiceCollection services) {
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration) {
			app.UseDeveloperExceptionPage();

			app.Run(async (context) => {
				string connectionString = ConfigurationExtensions.GetConnectionString(configuration, "DefaultConnection");
				await context.Response.WriteAsync(connectionString ?? "no connection string");
			});
		}
	}
}
