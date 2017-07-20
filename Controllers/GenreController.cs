using System;
using Music.Application;
using Music.Application.IService;
using Music.Infrastructure.Logging;
using Music.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Music.Controllers
{
    [Produces("application/json")]
    [Route("api/v3/genres")]
    public class GenreController : Controller
    {
        private readonly IGenreService GenreService;
        private readonly ILogger log = Logger.CreateLogger<GenreController>();

        public GenreController(IGenreService genreService)
        {
            GenreService = genreService;
        }

        [HttpPost]
        public IActionResult CreateGenre([FromBody] GenreDTO genreDTO)
        {
            try
            {
                GenreService.CreateGenre(genreDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("Connection error. " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGenre(long id)
        {
            try
            {
                GenreService.DeleteGenre(id);
            }
            catch (ArgumentException e)
            {
                log.LogError("Error deleting genre. ID not found. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpGet]
        public IActionResult GenAllGenres()
        {
            return Json(GenreService.GetAllGenres());
        }

        [HttpGet("{id}")]
        public IActionResult GetGenreById(long id)
        {
            try
            {
                return Json(GenreService.GetGenreById(id));
            }
            catch (ArgumentException e)
            {
                log.LogError("Error retrieving a genre. ID not found. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }
            catch (Exception e)
            {
                log.LogError("Connection error when retrieving genre. " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/AddTrackToGenre")]
        public IActionResult AddTrackToGenre(long id, [FromBody] TrackDTO trackDto)
        {
            try
            {
                GenreService.AddTrackToGenre(id, trackDto);
            }
            catch (ArgumentException e)
            {
                log.LogError("Error on adding a new book. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }
    }
}