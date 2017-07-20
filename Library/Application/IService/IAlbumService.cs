using System;
using System.Collections.Generic;
using Music.Presentation;
using Music.Domain.Entities;

namespace Music.Application.IService
{
    public interface IAlbumService
    {
        void DeleteAlbum(long id);
        ICollection<AlbumDTO> GetAllAlbums();
        AlbumDTO GetAlbumById(long id);
        void AddTrackToAlbum(long id, TrackDTO trackDTO);
        void AddArtistToAlbum(long id, ArtistDTO artistDTO);
        AlbumDTO MapAlbum(Album album);
        ICollection<AlbumDTO> MapAlbums(IEnumerable<Album> albums);
        void ModifyAlbum(long id, AlbumDTO albumDTO);
    }
}