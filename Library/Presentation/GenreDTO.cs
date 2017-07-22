using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class GenreDTO : BaseDTO
    {
        public ICollection<TrackDetailDTO> Tracks { get; set; }
        public string Type { get; set; }

        public GenreDTO()
        {
            Tracks = new LinkedList<TrackDetailDTO>();
        }

        public GenreDTO(long id, string type) : this()
        {
            Id = id;
            Type = type;
        }
    }
}