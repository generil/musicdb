using System;
using Music.Domain.Entities;

namespace Music.Domain.Associates
{
    public class ArtistAlbum
    {
        public long ArtistId { get; set; }
        public long AlbumId { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
    }
}