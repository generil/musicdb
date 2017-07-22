using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class AlbumDTO : BaseDTO
    {
        public ICollection<TrackDetailDTO> Tracks { get; set; }
        public ICollection<ArtistDetailDTO> Artists { get; set; }
        public string Title { get; set; }
        public string RecordLabel { get; set; }
        public DateTime ReleaseDate { get; set; }

        public AlbumDTO()
        {
            Tracks = new LinkedList<TrackDetailDTO>();
            Artists = new LinkedList<ArtistDetailDTO>();
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