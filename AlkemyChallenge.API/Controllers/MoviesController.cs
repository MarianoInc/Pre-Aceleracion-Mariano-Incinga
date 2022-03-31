using AlkemyChallenge.API.Dal;
using AlkemyChallenge.API.Interfaces;
using AlkemyChallenge.API.Models;
using AlkemyChallenge.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlkemyChallenge.API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MoviesController : ControllerBase
    {
        protected readonly IMovieRepository _movieRepository;
        
        public MoviesController(IMovieRepository movieRepository, DisneyWorldContext disneyWorldContext)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public IActionResult Get(int? genre, string name = "", string order = "")
        {
            var response = Filter(genre, name, order);

            return Ok(response);
        }

        //Obtener detalles
        [HttpGet]
        [Route(template: "{id}")]
        public IActionResult Details(int id)
        {
            if (_movieRepository.GetAllEntities().FirstOrDefault(movie => movie.MovieId == id) == null) { return BadRequest("El ícono no existe o el id no es válido"); }

            return Ok(_movieRepository.Get(id));
        }

        //Crear
        [HttpPost]
        public IActionResult Post(Movie movie)
        {
            if (movie == null) { return BadRequest("No se puede crear un ícono vacío"); }

            _movieRepository.Post(movie);
            return Ok(_movieRepository.GetAllEntities().ToList());
        }

        //Actualizar entidad
        [HttpPut]
        public IActionResult Put(Movie movie)
        {
            if (_movieRepository.GetAllEntities().FirstOrDefault(m => m.MovieId == movie.MovieId) == null) { return BadRequest("El ícono no existe"); }

            return Ok(_movieRepository.Update(movie));
        }

        [HttpDelete]
        [Route(template: "{id}")]
        public IActionResult Delete(int? id)
        {
            if (_movieRepository.GetAllEntities().FirstOrDefault(m => m.MovieId == id) == null) { return BadRequest("El ícono no existe o el id no es válido"); }
            return Ok(_movieRepository.Delete(id));
        }

        //Filtro del modelo de películas 
        private List<MovieResponseViewModel> Filter(int? genre, string name = "", string order = "")
        {
            var selectMethod = _movieRepository.GetAllEntities();
            var response = new List<MovieResponseViewModel>();

            if (!String.IsNullOrEmpty(name))
            {
                response = selectMethod.Where(movie => movie.Title.ToUpper() == name.ToUpper())
                                        .Select(movie => new MovieResponseViewModel
                                        {
                                            MovieImage = movie.MovieImage,
                                            Title = movie.Title,
                                            CreationDate = movie.CreationDate
                                        })
                                        .ToList();
            }
            else if (genre.HasValue)
            {
                response = selectMethod.Where(movie => movie.GenreId == genre)
                                        .Select(movie => new MovieResponseViewModel
                                        {
                                            MovieImage = movie.MovieImage,
                                            Title = movie.Title,
                                            CreationDate = movie.CreationDate
                                        })
                                        .ToList();
            }
            else
            {
                response = selectMethod.Select(movie => new MovieResponseViewModel
                {
                    MovieImage = movie.MovieImage,
                    Title = movie.Title,
                    CreationDate = movie.CreationDate
                }).ToList();
            }


            order = order.ToUpper() == "ASC" ? "DESC" : "ASC";
            var orderedList = from m in response select m;
                
            switch (order)
            {
                case "DESC":
                    orderedList = orderedList.OrderByDescending(m => m.CreationDate);
                    break;
                default:
                    orderedList = orderedList.OrderBy(m => m.CreationDate);
                    break;
            }

            response = orderedList.ToList();

            return response;

        }

    }
}
