using System.ComponentModel.DataAnnotations;

namespace MovieApplication.Models
{
	public class Movie
	{
		[Key]
		public string Id { get; set; }

		public string Title { get; set; }

		public string Genre { get; set; }

		public string ReleaseYear { get; set; }
	}
}
