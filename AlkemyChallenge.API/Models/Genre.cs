using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.API.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public string GenreImage { get; set; }
        //Relación con películas (Uno-a-muchos) 
        public int MovieId { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
