using System;
using System.Collections.Generic;
using AutoMapper;
using Music.Application.IService;
using Music.Domain.Entities;
using Music.Domain.IRepositories;
using Music.Domain.Factories;
using Music.Helper;
using Music.Presentation;

namespace Music.Application
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository AlbumRepository;
        private readonly IArtistRepository ArtistRepository;
        private readonly IMapper Mapper;

        public AlbumService(IAlbumRepository albumRepository, IArtistRepository artistRepository, IMapper mapper)
        {
            AlbumRepository = albumRepository;
            ArtistRepository = artistRepository;
            Mapper = mapper;
        }

        public void AddArtistToAlbum(long id, ArtistDTO artistDTO)
        {
            var album = AlbumRepository.Get(id);

            if (album == null)
            {
                throw new ArgumentException($"Album {id} not found.");
            }

            var artist = ArtistRepository.Get(artistDTO.Id);
            album.AddArtist(artist);

            AlbumRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void AddTrackToAlbum(long id, TrackDTO trackDTO)
        {
            var album = AlbumRepository.Get(id);
            Artist artist = new Artist();

            if (album == null)
            {
                throw new ArgumentException($"Album {id} does not exist");
            }

            Track track = artist.CreateTrack(trackDTO.Title, trackDTO.Duration, trackDTO.ReleaseDate);
            album.AddTrack(track);

            AlbumRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void DeleteAlbum(long id)
        {
            var album = AlbumRepository.Get(id);

            if (album == null)
            {
                throw new ArgumentException($"Album {id} does not exist.");
            }

            using (var transaction = AlbumRepository.UnitOfWork.BeginTransaction(2))
            {
                AlbumRepository.Remove(album);
                transaction.Commit();
            }
        }

        public AlbumDTO GetAlbumById(long id)
        {
            var album = AlbumRepository.Get(id);

            if (album == null)
            {
                throw new ArgumentException($"Album {id} doest not exist.");
            }

            return Mapper.Map<AlbumDTO>(MapAlbum(album));
        }

        public ICollection<AlbumDTO> GetAllAlbums()
        {
            var albums = AlbumRepository.GetAll();
            return !albums.IsNullOrEmpty() ? Mapper.Map<ICollection<AlbumDTO>>(MapAlbums(albums)) : null;
        }

        public AlbumDTO MapAlbum(Album album)
        {
            AlbumDTO albumDTO = new AlbumDTO(album.Id, album.Title, album.RecordLabel, album.ReleaseDate);

            foreach (var track in album.Tracks)
            {
                albumDTO.Tracks.Add(new TrackDTO(track.Track.Id, track.Track.Title, track.Track.Duration, track.Track.ReleaseDate));
            }

            foreach (var artist in album.Artists)
            {
                albumDTO.Artists.Add(new ArtistDTO(artist.Artist.Id, artist.Artist.Name, artist.Artist.Description, artist.Artist.Country, artist.Artist.YearActive));
            }

            return albumDTO;
        }

        public ICollection<AlbumDTO> MapAlbums(IEnumerable<Album> albums)
        {
            ICollection<AlbumDTO> albumDTOs = new LinkedList<AlbumDTO>();
            AlbumDTO albumDTO = null;

            foreach (var album in albums)
            {
                albumDTO = new AlbumDTO(album.Id, album.Title, album.RecordLabel, album.ReleaseDate);

                foreach (var track in album.Tracks)
                {
                    albumDTO.Tracks.Add(new TrackDTO(track.Track.Id, track.Track.Title, track.Track.Duration, track.Track.ReleaseDate));
                }

                foreach (var artist in album.Artists)
                {
                    albumDTO.Artists.Add(new ArtistDTO(artist.Artist.Id, artist.Artist.Name, artist.Artist.Description, artist.Artist.Country, artist.Artist.YearActive));
                }

                albumDTOs.Add(albumDTO);
            }

            return albumDTOs;
        }

        public void ModifyAlbum(long id, AlbumDTO albumDTO)
        {
            var album = AlbumRepository.Get(id);

            if (album == null)
            {
                throw new ArgumentException($"Artist {id} not found.");
            }

            album.ChangeDetails(new Album(albumDTO.Title, albumDTO.RecordLabel, albumDTO.ReleaseDate));

            using (var t = AlbumRepository.UnitOfWork.BeginTransaction(2))
            {
                AlbumRepository.Modify(album);
                t.Commit();
            }
        }
    }
}