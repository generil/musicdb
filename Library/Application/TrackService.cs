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
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository TrackRepository;
        private readonly IArtistRepository ArtistRepository;
        private readonly IAlbumRepository AlbumRepository;
        private readonly IGenreRepository GenreRepository;
        private readonly IMapper Mapper;

        public TrackService(ITrackRepository trackRepository, IArtistRepository artistRepository, IAlbumRepository albumRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            TrackRepository = trackRepository;
            ArtistRepository = artistRepository;
            GenreRepository = genreRepository;
            AlbumRepository = albumRepository;
            Mapper = mapper;
        }

        public void AddAlbumToTrack(long id, AlbumDTO albumDTO)
        {
            var track = TrackRepository.Get(id);

            if (track == null)
            {
                throw new ArgumentException($"Track {id} not found");
            }

            var album = AlbumRepository.Get(albumDTO.Id);
            track.AddAlbum(album);

            TrackRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void AddArtistToTrack(long id, ArtistDTO artistDTO)
        {
            var track = TrackRepository.Get(id);

            if (track == null)
            {
                throw new ArgumentException($"Track {id} not found");
            }

            var artist = ArtistRepository.Get(artistDTO.Id);
            track.AddArtist(artist);

            TrackRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void AddGenreToTrack(long id, GenreDTO genreDTO)
        {
            var track = TrackRepository.Get(id);

            if (track == null)
            {
                throw new ArgumentException($"Track {id} not found");
            }

            var genre = GenreRepository.Get(genreDTO.Id);
            track.AddGenre(genre);

            TrackRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void DeleteTrack(long id)
        {
            var track = TrackRepository.Get(id);

            if (track == null)
            {
                throw new ArgumentException($"Track {id} not found");
            }

            using (var transaction = TrackRepository.UnitOfWork.BeginTransaction(2))
            {
                TrackRepository.Remove(track);
                transaction.Commit();
            }
        }

        public ICollection<TrackDTO> GetAllTracks()
        {
            var tracks = TrackRepository.GetAll();
            return !tracks.IsNullOrEmpty() ? Mapper.Map<ICollection<TrackDTO>>(MapTracks(tracks)) : null;
        }

        public TrackDTO GetTrackById(long id)
        {
            var track = TrackRepository.Get(id);

            if (track == null)
            {
                throw new ArgumentException($"Track {id} not found.");
            }

            return Mapper.Map<TrackDTO>(MapTrack(track));
        }

        public TrackDTO MapTrack(Track track)
        {
            TrackDTO trackDTO = new TrackDTO(track.Id, track.Title, track.Duration, track.ReleaseDate);

            foreach (var album in track.Albums)
            {
                trackDTO.Albums.Add(new AlbumDetailDTO(album.Album.Title, album.Album.RecordLabel, album.Album.ReleaseDate));
            }

            foreach (var artist in track.Artists)
            {
                trackDTO.Artists.Add(new ArtistDetailDTO(artist.Artist.Name, artist.Artist.Description, artist.Artist.Country, artist.Artist.YearActive));
            }

            foreach (var genre in track.Genres)
            {
                trackDTO.Genres.Add(new GenreDetailDTO(genre.Genre.Type));
            }

            return trackDTO;
        }

        public ICollection<TrackDTO> MapTracks(IEnumerable<Track> tracks)
        {
            ICollection<TrackDTO> trackDTOs = new LinkedList<TrackDTO>();
            TrackDTO trackDTO = null;

            foreach (var track in tracks)
            {
                trackDTO = new TrackDTO(track.Id, track.Title, track.Duration, track.ReleaseDate);

                foreach (var album in track.Albums)
                {
                    trackDTO.Albums.Add(new AlbumDetailDTO(album.Album.Title, album.Album.RecordLabel, album.Album.ReleaseDate));
                }

                foreach (var artist in track.Artists)
                {
                    trackDTO.Artists.Add(new ArtistDetailDTO(artist.Artist.Name, artist.Artist.Description, artist.Artist.Country, artist.Artist.YearActive));
                }

                foreach (var genre in track.Genres)
                {
                    trackDTO.Genres.Add(new GenreDetailDTO(genre.Genre.Type));
                }

                trackDTOs.Add(trackDTO);
            }

            return trackDTOs;
        }
    }
}