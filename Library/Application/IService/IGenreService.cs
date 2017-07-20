using System;
using System.Collections.Generic;
using Music.Presentation;
using Music.Domain.Entities;

namespace Music.Application.IService
{
    public interface IGenreService
    {
        void CreateGenre(GenreDTO genreDTO);
        void DeleteGenre(long id);
        GenreDTO GetGenreById(long id);
        ICollection<GenreDTO> GetAllGenres();
        void AddTrackToGenre(long id, TrackDTO trackDTO);
        GenreDTO MapGenre(Genre genre);
        ICollection<GenreDTO> MapGenres(IEnumerable<Genre> genres);
    }
}