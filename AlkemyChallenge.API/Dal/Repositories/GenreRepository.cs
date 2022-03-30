using AlkemyChallenge.API.Interfaces;
using AlkemyChallenge.API.Models;

namespace AlkemyChallenge.API.Dal.Repositories
{
    public class GenreRepository : BaseRepository<Genre, DisneyWorldContext>, IGenreRepository
    {
        public GenreRepository(DisneyWorldContext context) : base(context)
        {
        }
    }
}
