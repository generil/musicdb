using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class TrackDTO : BaseDTO
    {
        public ICollection<ArtistDTO> Artists { get; set; }
        public ICollection<AlbumDTO> Albums { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public TrackDTO()
        {
            Artists = new LinkedList<ArtistDTO>();
            Albums = new LinkedList<AlbumDTO>();
            Genres = new LinkedList<GenreDTO>();
        }

        public TrackDTO(long id, string title, int duration, DateTime release) : this()
        {
            Id = id;
            Title = title;
            Duration = duration;
            ReleaseDate = release;
        }
    }
}