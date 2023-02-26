using MovieApplication.Data;
using MovieApplication.Common.Interfaces;
using MovieApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieApplication.Services
{
	public class MovieService : IMovieService
	{
		private readonly MovieDbContext _movieContext;

		public MovieService(MovieDbContext dbContext)
		{
			_movieContext = dbContext;
		}
		public IEnumerable<Movie> GetMoviewByGenre(string genre)
		{
			IEnumerable<Movie> movies = Enumerable.Empty<Movie>();

			try
			{
				movies = _movieContext.movies.Where(movie => movie.Genre.Equals(genre)).ToListAsync().GetAwaiter().GetResult();
			}
			catch
			{
				throw;
			}

			return movies;
		}
	}
}
