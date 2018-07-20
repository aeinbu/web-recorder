using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Middleware;
using Middleware.Storage;

namespace Web
{
	public class Startup
	{
		private IConfigurationRoot _configuration;

		public Startup(IHostingEnvironment env, ILogger<Startup> logger)
		{
			logger.LogInformation($"ConfigurationBasePath={env.ContentRootPath}");

			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.SetBasePath(env.ContentRootPath);
			configurationBuilder.AddJsonFile("appsettings.json");
			_configuration = configurationBuilder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();

			//TODO: Find more elegant way for configuring...
			services.ConfigureRecorder(_configuration);
			services.ConfigurePlayer(_configuration);
			services.ConfigureHttpProxy(_configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services, ILogger<Startup> logger)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// app.UseRecorder();
			// app.UsePlayer();
			app.UseHttpProxy();
			
			app.Run(async (context) =>
			{
				//TODO: Make a middleware for default response!? Or have a default response from Player? Or use existing middleware...
				await context.Response.WriteAsync("app.Run: Thank you! Your request was recorded...");
			});
		}

	}
}
