using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApplication.Common.Interfaces;
using MovieApplication.Common.Middleware;
using MovieApplication.Data;
using MovieApplication.Services;

namespace MovieApplication
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddLogging();

			// Add entity framework
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			builder.Services.AddDbContextPool<MovieDbContext>(
								options => options.UseSqlServer(configuration.GetConnectionString("movieDB")));

			builder.Services.AddTransient<IMovieService, MovieService>();
			builder.Services.AddTransient<ILoggerFactory, LoggerFactory>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.UseMiddleware<ExceptionMiddleware>();

			app.Run();
		}
	}
}