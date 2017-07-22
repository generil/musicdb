using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class ArtistDTO : RecordDTO
    {
        public ICollection<AlbumDetailDTO> Albums { get; set; }
        public ICollection<TrackDetailDTO> Tracks { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }

        public ArtistDTO()
        {
            Albums = new LinkedList<AlbumDetailDTO>();
            Tracks = new LinkedList<TrackDetailDTO>();
        }

        public ArtistDTO(long id, string name, string description, string country, int year) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            Country = country;
            Year = year;
        }
    }
}