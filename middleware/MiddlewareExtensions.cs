using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Storage;

namespace Middleware
{
	public static class MiddlewareExtensions
	{
		public static void ConfigureRecorder(this IServiceCollection services, IConfigurationRoot configuration)
		{
			services.Configure<RecorderStore.Options>(configuration.GetSection("RecorderStore"));
			services.Configure<Recorder.Options>(configuration.GetSection("Recorder"));
			services.AddScoped<IRecorderStore, RecorderStore>();
			services.AddScoped<RecorderMiddleware>();
			services.AddScoped<Recorder>();
		}

		public static void ConfigurePlayer(this IServiceCollection services, IConfigurationRoot configuration)
		{
			services.Configure<PlayerStore.Options>(configuration.GetSection("PlayerStore"));
			services.AddScoped<IPlayerStore, PlayerStore>();
			services.AddScoped<PlayerMiddleware>();
			services.AddScoped<Player>();
		}

		public static IApplicationBuilder UseRecorder(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<RecorderMiddleware>();
		}

		public static IApplicationBuilder UsePlayer(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<PlayerMiddleware>();
		}
	}
}