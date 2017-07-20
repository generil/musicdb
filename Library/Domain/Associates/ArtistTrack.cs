using System;
using Music.Domain.Entities;

namespace Music.Domain.Associates
{
    public class ArtistTrack
    {
        public long ArtistId { get; set; }
        public long TrackId { get; set; }
        public Artist Artist { get; set; }
        public Track Track { get; set; }
    }
}