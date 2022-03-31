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
    [Route("api/characters")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CharactersController : ControllerBase
    {
        protected readonly ICharacterRepository _characterRepository;

        public CharactersController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        [HttpGet]
        public IActionResult Get(int? age, /*int? movies,*/ string name = "")
        {
            var response = Filter(age, /*movies,*/ name);
            return Ok(response);
        }

        //Obtener detalles
        [HttpGet]
        [Route(template: "{id}")]
        public IActionResult Details(int id)
        {
            if (_characterRepository.GetAllEntities().FirstOrDefault(c => c.CharacterId == id) == null) { return BadRequest("El ícono no existe o el id no es válido"); }

            return Ok(_characterRepository.Get(id));
        }

        //Crear entidad
        [HttpPost]
        public IActionResult Post(Character character)
        {
            if (character == null) { return BadRequest("No se puede crear un ícono vacío"); }
            
            _characterRepository.Post(character);
            return Ok(_characterRepository.GetAllEntities().ToList());
        }

        //Actualizar entidad
        [HttpPut]
        public IActionResult Put(Character character)
        {
            if (_characterRepository.GetAllEntities().FirstOrDefault(c => c.CharacterId == character.CharacterId) == null) { return BadRequest("El ícono no existe"); }

            return Ok(_characterRepository.Update(character));
        }

        //Borrar entidad
        [HttpDelete]
        [Route(template: "{id}")]
        public IActionResult Delete(int? id)
        {
            if (_characterRepository.GetAllEntities().FirstOrDefault(c => c.CharacterId == id) == null) { return BadRequest("El ícono no existe o el id no es válido"); }

            return Ok(_characterRepository.Delete(id));
        }

        //Filtro del modelo de Personajes
        private List<CharacterResponseViewModel> Filter(int? age, /*int? movies,*/ string name = "")
        {
            var selectMethod = _characterRepository.GetAllEntities();
            var response = new List<CharacterResponseViewModel>();
            if (!String.IsNullOrEmpty(name))
            {
                response = selectMethod.Where(c => c.CharacterName.ToUpper() == name.ToUpper())
                                        .Select(c => new CharacterResponseViewModel
                                        {
                                            CharacterImage = c.CharacterImage,
                                            CharacterName = c.CharacterName
                                        })
                                        .ToList();
            }
            else if (age.HasValue)
            {
                response = selectMethod.Where(c => c.Age == age)
                                        .Select(c => new CharacterResponseViewModel
                                        {
                                            CharacterImage = c.CharacterImage,
                                            CharacterName = c.CharacterName
                                        })
                                        .ToList();
            }
            //No fue probado.
            //else if (movies.HasValue)
            //{

            //    response = selectMethod.Where(c => c.Movies.Any(m => m.MovieId == movies))
            //                            .Select(c => new CharacterResponseViewModel
            //                            {
            //                                CharacterImage = c.CharacterImage,
            //                                CharacterName = c.CharacterName
            //                            }).ToList();
            //}
            else
            {
                response = selectMethod.Select(c => new CharacterResponseViewModel
                {
                    CharacterImage = c.CharacterImage,
                    CharacterName = c.CharacterName
                }).ToList();
            }
            return response;
        }
    }
}
