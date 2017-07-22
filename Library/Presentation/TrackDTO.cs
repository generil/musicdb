using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class TrackDTO : BaseDTO
    {
        public ICollection<ArtistDetailDTO> Artists { get; set; }
        public ICollection<AlbumDetailDTO> Albums { get; set; }
        public ICollection<GenreDetailDTO> Genres { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public TrackDTO()
        {
            Artists = new LinkedList<ArtistDetailDTO>();
            Albums = new LinkedList<AlbumDetailDTO>();
            Genres = new LinkedList<GenreDetailDTO>();
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