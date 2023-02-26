using Microsoft.AspNetCore.Mvc;
using MovieApplication.Common.Interfaces;
using MovieApplication.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MovieApplication.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MovieController : ControllerBase
	{
		private readonly ILogger<MovieController> _logger;
		private readonly IMovieService _service;

		public MovieController(ILogger<MovieController> logger, IMovieService movieService)
		{
			_logger = logger;
			_service = movieService;
		}

		[HttpGet]
		[Route("/movies")]
		public IEnumerable<Movie> Get([BindRequired][FromQuery] string genre)
		{
			return _service.GetMoviewByGenre(genre);
		}
	}
}