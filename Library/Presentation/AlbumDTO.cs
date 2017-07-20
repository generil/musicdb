using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class AlbumDTO : BaseDTO
    {
        public ICollection<TrackDTO> Tracks { get; set; }
        public ICollection<ArtistDTO> Artists { get; set; }
        public string Title { get; set; }
        public string RecordLabel { get; set; }
        public DateTime ReleaseDate { get; set; }

        public AlbumDTO()
        {
            Tracks = new LinkedList<TrackDTO>();
            Artists = new LinkedList<ArtistDTO>();
        }

        public AlbumDTO(long id, string title, string label, DateTime release) : this()
        {
            Id = id;
            Title = title;
            RecordLabel = label;
            ReleaseDate = release;
        }
    }
}