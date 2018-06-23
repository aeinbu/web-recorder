using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Web
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var requestStorePath = "../temp/requests";
			services.AddScoped<Storage.IRequestStore>(x => new Storage.RequestStore(requestStorePath));

			var responseStorePath = "../temp/responses";
			services.AddScoped<Storage.IResponseStore>(x => new Storage.ResponseStore(responseStorePath));

			services.AddScoped<Storage.Recorder>();
			services.AddScoped<Storage.Player>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services, ILogger<Startup> logger)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//TODO: Resolve middleware inside app.Run?
			var recorder = services.GetService<Storage.Recorder>();
			var player = services.GetService<Storage.Player>();

			app.Run(async (context) =>
			{
				await recorder.Handle(context.Request);
				var responseStream = await player.Handle(context.Request);
				responseStream.CopyTo(context.Response.Body);
			});
		}

	}
}
