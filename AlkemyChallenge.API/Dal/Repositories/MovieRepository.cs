using AlkemyChallenge.API.Interfaces;
using AlkemyChallenge.API.Models;

namespace AlkemyChallenge.API.Dal.Repositories
{
    public class MovieRepository : BaseRepository<Movie, DisneyWorldContext>, IMovieRepository
    {
        public MovieRepository(DisneyWorldContext context) : base(context)
        {
        }
    }
}
