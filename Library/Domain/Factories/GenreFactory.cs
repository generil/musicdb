using System;
using Music.Domain.Entities;

namespace Music.Domain.Factories
{
    public static class GenreFactory
    {
        public static Genre CreateGenre(string type)
        {
            Genre genre = new Genre(type);
            return genre;
        }
    }
}