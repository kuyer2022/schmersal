using MovieApplication.Models;

namespace MovieApplication.Common.Interfaces
{
	public interface IMovieService
	{
		IEnumerable<Movie> GetMoviewByGenre(string genre);
	}
}
