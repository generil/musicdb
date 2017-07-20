using System;
using System.Collections.Generic;
using Music.Presentation;
using Music.Domain.Entities;

namespace Music.Application.IService
{
    public interface ITrackService
    {
        void DeleteTrack(long id);
        TrackDTO GetTrackById(long id);
        ICollection<TrackDTO> GetAllTracks();
        void AddAlbumToTrack(long id, AlbumDTO albumDTO);
        void AddArtistToTrack(long id, ArtistDTO artistDTO);
        void AddGenreToTrack(long id, GenreDTO genreDTO);
        TrackDTO MapTrack(Track track);
        ICollection<TrackDTO> MapTracks(IEnumerable<Track> tracks);
    }
}