using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.API.Models
{
    public class Character
    {
        [Key]
        public int CharacterId { get; set; }
        public string CharacterImage { get; set; }
        public string CharacterName { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string History { get; set; }
        //Falta relación con peliculas (muchos a muchos)
        public ICollection<CharacterMovie> Movies { get; set; }

    }
}
