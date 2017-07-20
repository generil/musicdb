using System;
using System.Collections.Generic;
using Music.Presentation;
using Music.Domain.Entities;

namespace Music.Application.IService
{
    public interface IArtistService
    {
        void CreateArtist(ArtistDTO artist);
        void DeleteArtist(long id);
        ICollection<ArtistDTO> GetAllArtists();
        ArtistDTO GetArtistById(long id);
        void ModifyArtist(long id, ArtistDTO artistDTO);
        void AddAlbumToArtist(long id, AlbumDTO albumDTO);
        void AddTrackToArtist(long id, TrackDTO trackDTO);
        ArtistDTO MapArtist(Artist artist);
        ICollection<ArtistDTO> MapArtists(IEnumerable<Artist> artists);
    }
}