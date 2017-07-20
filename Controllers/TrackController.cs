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
    [Route("api/v3/tracks")]
    public class TrackController : Controller
    {
        private readonly ITrackService TrackService;
        private readonly ILogger log = Logger.CreateLogger<TrackController>();

        public TrackController(ITrackService trackService)
        {
            TrackService = trackService;
        }

        [HttpPut("{id}/AddArtistToTrack")]
        public IActionResult AddArtistToTrack(long id, [FromBody] ArtistDTO artistDTO)
        {
            try
            {
                TrackService.AddArtistToTrack(id, artistDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("Error on adding a new book. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpPut("{id}/AddAlbumToTrack")]
        public IActionResult AddAlbumToTrack(long id, [FromBody] AlbumDTO albumDTO)
        {
            try
            {
                TrackService.AddAlbumToTrack(id, albumDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("Error on adding a new book. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpPut("{id}/AddGenreToTrack")]
        public IActionResult AddGenreToTrack(long id, [FromBody] GenreDTO genreDTO)
        {
            try
            {
                TrackService.AddGenreToTrack(id, genreDTO);
            }
            catch (ArgumentException e)
            {
                log.LogError("Error on adding a new book. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTrack(long id)
        {
            try
            {
                TrackService.DeleteTrack(id);
            }

            catch (ArgumentException e)
            {
                log.LogError("Error deleting a track. ID not found. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }

            return Ok(StatusCodes.Status200OK);
        }

        [HttpGet]
        public IActionResult GetAllTracks()
        {
            return Json(TrackService.GetAllTracks());
        }

        [HttpGet("{id}")]
        public IActionResult GetTrackById(long id)
        {
            try
            {
                return Json(TrackService.GetTrackById(id));
            }
            catch (ArgumentException e)
            {
                log.LogError("Error retrieving a track. ID not found. " + e.Message);
                return NotFound(StatusCodes.Status404NotFound);
            }
            catch (Exception e)
            {
                log.LogError("Connection error when retrieving a track. " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}