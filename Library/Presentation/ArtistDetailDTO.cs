using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class ArtistDetailDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }

        public ArtistDetailDTO()
        {
        }

        public ArtistDetailDTO(string name, string description, string country, int year) : this()
        {
            Name = name;
            Description = description;
            Country = country;
            Year = year;
        }
    }
}