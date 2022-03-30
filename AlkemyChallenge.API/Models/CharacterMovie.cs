namespace AlkemyChallenge.API.Models
{
    public class CharacterMovie
    {
        //Clave Foránea de personajes
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        //Clave Foránea de géneros
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
