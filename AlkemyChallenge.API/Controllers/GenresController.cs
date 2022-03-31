using AlkemyChallenge.API.Interfaces;
using AlkemyChallenge.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AlkemyChallenge.API.Controllers
{
    [Route("api/genres")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GenresController : ControllerBase
    {
        protected readonly IGenreRepository _genreRepository;
        public GenresController(IGenreRepository genreRepository)
        {
                _genreRepository = genreRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var selectMethod = _genreRepository.GetAllEntities().ToList();

            return Ok(selectMethod);
        }

        [HttpGet]
        [Route(template: "{id}")]
        public IActionResult Details(int id)
        {
            if (_genreRepository.GetAllEntities().FirstOrDefault(g => g.GenreId == id) == null) { return BadRequest("El ícono no existe o el id no es válido"); }
            return Ok(_genreRepository.Get(id));
        }

        //Crear
        [HttpPost]
        public IActionResult Post(Genre genre)
        {
            _genreRepository.Post(genre);
            return Ok(_genreRepository.GetAllEntities().ToList());
        }

        //Modificar
        [HttpPut]
        public IActionResult Put(Genre genre)
        {
            if (_genreRepository.GetAllEntities().FirstOrDefault(g => g.GenreId == genre.GenreId) == null) { return BadRequest("El ícono no existe o el id no es válido"); }
            return Ok(_genreRepository.Update(genre));
        }

        //Borrar
        [HttpDelete]
        [Route(template: "{id}")]
        public IActionResult Delete(int? id)
        {
            if (_genreRepository.GetAllEntities().FirstOrDefault(g => g.GenreId == id) == null) { return BadRequest("El ícono no existe o el id no es válido"); }
            return Ok(_genreRepository.Delete(id));
        }





    }
}
