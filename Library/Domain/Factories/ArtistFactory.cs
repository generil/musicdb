using System;
using Music.Domain.Entities;

namespace Music.Domain.Factories
{
    public static class ArtistFactory
    {
        public static Artist CreateArtist(string name, string description, string country, int year)
        {
            Artist artist = new Artist(name, description, country, year);

            return artist;
        }
    }
}