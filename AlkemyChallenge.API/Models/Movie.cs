using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.API.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string MovieImage { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int Rating { get; set; }
        //Relación con género (Uno-a-muchos)
        public int GenreId { get; set; }
        public Genre Genres  { get; set; }

        //Falta la relación con los personajes (Muchos-a-muchos) 
        public ICollection<CharacterMovie> Characters { get; set; }
    }
}
