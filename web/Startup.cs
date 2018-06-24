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
			services.Configure<RequestStore.Options>(_configuration.GetSection("RequestStore.Options"));
			services.Configure<ResponseStore.Options>(_configuration.GetSection("ResponseStore.Options"));
			services.AddScoped<IRequestStore, RequestStore>();
			services.AddScoped<IResponseStore, ResponseStore>();
			services.AddScoped<Recorder>();
			services.AddScoped<Player>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services, ILogger<Startup> logger)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//TODO: Resolve middleware inside app.Run?
			var recorder = services.GetService<Recorder>();
			var player = services.GetService<Player>();

			//TODO: Follow proper middleware patterns!!!
			// app.Use(recorder.Handle???);
			// app.Use(player.Handle???);
			
			app.Run(async (context) =>
			{
				await recorder.Handle(context.Request);
				using (var responseStream = await player.Handle(context.Request))
				{
					responseStream.CopyTo(context.Response.Body);
				}
			});
		}

	}
}
