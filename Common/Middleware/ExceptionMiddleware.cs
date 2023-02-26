using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace MovieApplication.Common.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private ILogger<ExceptionMiddleware> _logger;

		public ExceptionMiddleware(ILoggerFactory loggerFactory, RequestDelegate next)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				context.Response.ContentType = "application/json";

				var errorResponse = new ErrorResponse();

				switch (ex.InnerException)
				{
					case SqlException:
						context.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
						errorResponse.Message= "Database exception";
						break;

					default:
						context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						errorResponse.Message = "Application Exception";
						break;
				}

				_logger.LogCritical(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);

				await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
			}
		}

		private class ErrorResponse 
		{ 
			public string Message { get; set; }
		}
	}
}
