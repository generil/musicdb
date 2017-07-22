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
    [Route("api/v3/albums")]
    public class AlbumController : Controller
    {
        private readonly IAlbumService AlbumService;
        private readonly ILogger log = Logger.CreateLogger<AlbumController>();

        public AlbumController(IAlbumService albumService)
        {
            AlbumService = albumService;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAlbum(long id)
        {
            try
            {
                AlbumService.DeleteAlbum(id);
            }
            catch (ArgumentException e)
            {
                log.LogError("The album with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpGet]
        public IActionResult GetAllAlbums()
        {
            return Json(AlbumService.GetAllAlbums());
        }

        [HttpGet("{id}")]
        public IActionResult GetAlbumById(long id)
        {
            try
            {
                return Json(AlbumService.GetAlbumById(id));
            }
            catch (ArgumentException e)
            {
                log.LogError("The album with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }
            catch (Exception e)
            {
                log.LogError("Something happened when getting an album. " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/AddArtistToAlbum")]
        public IActionResult AddArtistToAlbum(long id, [FromBody] ArtistDTO artistDTO)
        {
            try
            {
                AlbumService.AddArtistToAlbum(id, artistDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("The album with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpPut("{id}/AddTrackToAlbum")]
        public IActionResult AddTrackToAlbum(long id, [FromBody] TrackDTO trackDTO)
        {
            try
            {
                AlbumService.AddTrackToAlbum(id, trackDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("The album with the id :" + id + " does not exist." + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }
    }
}