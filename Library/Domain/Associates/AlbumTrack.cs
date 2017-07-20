using System;
using Music.Domain.Entities;

namespace Music.Domain.Associates
{
    public class AlbumTrack
    {
        public long AlbumId { get; set; }
        public long TrackId { get; set; }
        public Album Album { get; set; }
        public Track Track { get; set; }
    }
}