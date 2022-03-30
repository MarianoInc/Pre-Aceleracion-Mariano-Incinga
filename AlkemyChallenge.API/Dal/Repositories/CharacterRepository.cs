using AlkemyChallenge.API.Interfaces;
using AlkemyChallenge.API.Models;

namespace AlkemyChallenge.API.Dal.Repositories
{
    public class CharacterRepository : BaseRepository<Character, DisneyWorldContext>, ICharacterRepository
    {
        public CharacterRepository(DisneyWorldContext context) : base(context)
        {
        }
    }
}
