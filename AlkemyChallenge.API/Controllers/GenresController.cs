using AlkemyChallenge.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlkemyChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        protected readonly IGenreRepository _genreRepository;
        public GenresController(IGenreRepository genreRepository)
        {
                _genreRepository = genreRepository;
        }






    }
}
