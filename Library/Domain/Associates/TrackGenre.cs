using System;
using Music.Domain.Entities;

namespace Music.Domain.Associates
{
    public class TrackGenre
    {
        public long TrackId { get; set; }
        public long GenreId { get; set; }
        public Track Track { get; set; }
        public Genre Genre { get; set; }
    }
}