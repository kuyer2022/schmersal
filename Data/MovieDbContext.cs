namespace MovieApplication.Data
{
	using Microsoft.EntityFrameworkCore;
	using Models;

	public class MovieDbContext : DbContext
	{
		public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
		{
		}

		public DbSet<Movie> movies { get; set; }
	}
}
