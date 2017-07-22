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
    [Route("api/v3/artists")]
    public class ArtistController : Controller
    {
        private readonly IArtistService ArtistService;
        private readonly ILogger log = Logger.CreateLogger<ArtistController>();

        public ArtistController(IArtistService artistService)
        {
            ArtistService = artistService;
        }

        [HttpPost]
        public IActionResult CreateArtist([FromBody] ArtistDTO artistDTO)
        {
            try
            {
                ArtistService.CreateArtist(artistDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("Something happened when creating an artist. " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpGet]
        public IActionResult GetAllArtists()
        {
            return Json(ArtistService.GetAllArtists());
        }

        [HttpGet("{id}")]
        public IActionResult GetArtistById(long id)
        {
            try
            {
                return Json(ArtistService.GetArtistById(id));
            }
            catch (ArgumentException e)
            {
                log.LogError("An artist with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }
            catch (Exception e)
            {
                log.LogError("Something happened when retrieving an artist. " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult ModifyArtist(long id, [FromBody] ArtistDTO artistDTO)
        {
            try
            {
                ArtistService.ModifyArtist(id, artistDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("An artist with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }
            catch (Exception e)
            {
                log.LogError("Something happened when modifying an artist. " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArtist(long id)
        {
            try
            {
                ArtistService.DeleteArtist(id);
            }
            catch (ArgumentException e)
            {
                log.LogError("An artist with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpPut("{id}/AddTrackToArtist")]
        public IActionResult AddTrackToArtist(long id)
        {
            DateTime now = DateTime.Now;
            TrackDTO trackDTO = new TrackDTO(1, "My Collection", 415, now);
            try
            {
                ArtistService.AddTrackToArtist(id, trackDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("An artist with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpPut("{id}/AddAlbumToArtist")]
        public IActionResult AddAlbumToArtist(long id, [FromBody] AlbumDTO albumDTO)
        {
            try
            {
                ArtistService.AddAlbumToArtist(id, albumDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("An artist with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }
    }
}
