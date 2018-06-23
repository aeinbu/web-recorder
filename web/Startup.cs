using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace web
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.Run(async (context) =>
			{
                var path = context.Request.Path.ToString();
                var querystring = context.Request.QueryString;
                var protocol = context.Request.Protocol;    // "HTTP/1.1"
                var scheme = context.Request.Scheme;        // "https" or https"
				var method = context.Request.Method;        // "GET", "POST", "PUT", "DELETE" etc
				var headers = string.Concat(context.Request.Headers.Select(hdr => $"{hdr.Key}: {hdr.Value}\r\n"));
				var body = await ResolveStreamToString(context.Request.Body);

				var response = $"This was the request made:\r\n\r\n{method} {scheme}://servername:port{path}{querystring} {protocol}\r\n{headers}\r\n{body}";
				await context.Response.WriteAsync(response);
			});
		}

		private async Task<string> ResolveStreamToString(Stream stream)
		{
			using (var sr = new StreamReader(stream))
			{
                return await sr.ReadToEndAsync();
			}
		}
	}
}
