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
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository GenreRepository;
        private readonly ITrackRepository TrackRepository;
        private readonly IMapper Mapper;

        public GenreService(IGenreRepository genreRepository, ITrackRepository trackRepository, IMapper mapper)
        {
            GenreRepository = genreRepository;
            TrackRepository = trackRepository;
            Mapper = mapper;
        }

        public void AddTrackToGenre(long id, TrackDTO trackDTO)
        {
            var genre = GenreRepository.Get(id);

            if (genre == null)
            {
                throw new ArgumentException($"Genre {id} not found.");
            }
            var track = TrackRepository.Get(trackDTO.Id);
            genre.AddTrack(track);
            GenreRepository.UnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void CreateGenre(GenreDTO genreDTO)
        {
            using (var transaction = GenreRepository.UnitOfWork.BeginTransaction(2))
            {
                GenreRepository.Add(GenreFactory.CreateGenre(genreDTO.Type));
                transaction.Commit();
            }
        }

        public void DeleteGenre(long id)
        {
            var genre = GenreRepository.Get(id);

            if (genre == null)
            {
                throw new ArgumentException($"Genre {id} doest not exist.");
            }

            using (var transaction = GenreRepository.UnitOfWork.BeginTransaction(2))
            {
                GenreRepository.Remove(genre);
                transaction.Commit();
            }
        }

        public ICollection<GenreDTO> GetAllGenres()
        {
            var genres = GenreRepository.GetAll();
            return !genres.IsNullOrEmpty() ? Mapper.Map<ICollection<GenreDTO>>(MapGenres(genres)) : null;
        }

        public GenreDTO GetGenreById(long id)
        {
            var genre = GenreRepository.Get(id);

            if (genre == null)
            {
                throw new ArgumentException($"Genre {id} not found.");
            }

            return Mapper.Map<GenreDTO>(MapGenre(genre));
        }

        public GenreDTO MapGenre(Genre genre)
        {
            GenreDTO GenreDTO = new GenreDTO(genre.Id, genre.Type);

            foreach (var track in genre.Tracks)
            {
                GenreDTO.Tracks.Add(new TrackDetailDTO(track.Track.Title, track.Track.Duration, track.Track.ReleaseDate));
            }

            return GenreDTO;
        }

        public ICollection<GenreDTO> MapGenres(IEnumerable<Genre> genres)
        {
            ICollection<GenreDTO> GenreDTOs = new LinkedList<GenreDTO>();
            GenreDTO GenreDTO = null;

            foreach (var genre in genres)
            {
                GenreDTO = new GenreDTO(genre.Id, genre.Type);

                foreach (var track in genre.Tracks)
                {
                    GenreDTO.Tracks.Add(new TrackDetailDTO(track.Track.Title, track.Track.Duration, track.Track.ReleaseDate));
                }

                GenreDTOs.Add(GenreDTO);
            }

            return GenreDTOs;
        }
    }
}