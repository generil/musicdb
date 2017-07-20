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
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository ArtistRepository;
        private readonly IMapper Mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper)
        {
            ArtistRepository = artistRepository;
            Mapper = mapper;
        }
        public void AddAlbumToArtist(long id, AlbumDTO albumDTO)
        {
            var artist = ArtistRepository.Get(id);

            if (artist == null)
            {
                throw new ArgumentException($"Artist {id} not found.");
            }

            Album album = artist.CreateAlbum(albumDTO.Title, albumDTO.RecordLabel, albumDTO.ReleaseDate);
            artist.AddAlbum(album);

            ArtistRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void AddTrackToArtist(long id, TrackDTO trackDTO)
        {
            var artist = ArtistRepository.Get(id);

            if (artist == null)
            {
                throw new ArgumentException($"Artist {id} not found.");
            }

            Track track = artist.CreateTrack(trackDTO.Title, trackDTO.Duration, trackDTO.ReleaseDate);
            artist.AddTrack(track);

            ArtistRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void CreateArtist(ArtistDTO artistDTO)
        {
            using (var transaction = ArtistRepository.UnitOfWork.BeginTransaction(2))
            {
                ArtistRepository.Add(ArtistFactory.CreateArtist(artistDTO.Name, artistDTO.Description, artistDTO.Country, artistDTO.Year));
                transaction.Commit();
            }
        }

        public void DeleteArtist(long id)
        {
            var artist = ArtistRepository.Get(id);

            if (artist == null)
            {
                throw new ArgumentException($"Artist {id} doest not exist.");
            }

            using (var transaction = ArtistRepository.UnitOfWork.BeginTransaction(2))
            {
                ArtistRepository.Remove(artist);
                transaction.Commit();
            }
        }

        public ICollection<ArtistDTO> GetAllArtists()
        {
            var artists = ArtistRepository.GetAll();
            return !artists.IsNullOrEmpty() ? Mapper.Map<ICollection<ArtistDTO>>(MapArtists(artists)) : null;
        }

        public ArtistDTO GetArtistById(long id)
        {
            var artist = ArtistRepository.Get(id);

            if (artist == null)
            {
                throw new ArgumentException($"Artist {id} not found.");
            }

            return Mapper.Map<ArtistDTO>(MapArtist(artist));
        }

        public ArtistDTO MapArtist(Artist artist)
        {
            ArtistDTO artistDTO = new ArtistDTO(artist.Id, artist.Name, artist.Description, artist.Country, artist.YearActive);

            foreach (var track in artist.Tracks)
            {
                artistDTO.Tracks.Add(new TrackDTO(track.Track.Id, track.Track.Title, track.Track.Duration, track.Track.ReleaseDate));
            }

            foreach (var album in artist.Albums)
            {
                artistDTO.Albums.Add(new AlbumDTO(album.Album.Id, album.Album.Title, album.Album.RecordLabel, album.Album.ReleaseDate));
            }

            return artistDTO;
        }

        public ICollection<ArtistDTO> MapArtists(IEnumerable<Artist> artists)
        {
            ICollection<ArtistDTO> artistDTOs = new LinkedList<ArtistDTO>();
            ArtistDTO artistDTO = null;

            foreach (var artist in artists)
            {
                artistDTO = new ArtistDTO(artist.Id, artist.Name, artist.Description, artist.Country, artist.YearActive);

                if (artist.Tracks.Count > 0)
                {
                    foreach (var track in artist.Tracks)
                    {
                        artistDTO.Tracks.Add(new TrackDTO(track.Track.Id, track.Track.Title, track.Track.Duration, track.Track.ReleaseDate));
                    }
                }

                if (artist.Albums.Count > 0)
                {
                    foreach (var album in artist.Albums)
                    {
                        artistDTO.Albums.Add(new AlbumDTO(album.Album.Id, album.Album.Title, album.Album.RecordLabel, album.Album.ReleaseDate));
                    }
                }

                artistDTOs.Add(artistDTO);
            }

            return artistDTOs;
        }

        public void ModifyArtist(long id, ArtistDTO artistDTO)
        {
            var artist = ArtistRepository.Get(id);

            if (artist == null)
            {
                throw new ArgumentException($"Artist {id} not found.");
            }

            artist.ChangeDetails(new Artist(artistDTO.Name, artistDTO.Description, artistDTO.Country, artistDTO.Year));

            using (var t = ArtistRepository.UnitOfWork.BeginTransaction(2))
            {
                ArtistRepository.Modify(artist);
                t.Commit();
            }
        }
    }
}