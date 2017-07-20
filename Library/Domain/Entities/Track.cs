using System;
using System.Collections.Generic;
using Music.Domain.Associates;

namespace Music.Domain.Entities
{
    public class Track : Entity
    {
        public ICollection<ArtistTrack> Artists { get; set; }
        public ICollection<TrackGenre> Genres { get; set; }
        public ICollection<AlbumTrack> Albums { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Track()
        {
            Artists = new LinkedList<ArtistTrack>();
            Genres = new LinkedList<TrackGenre>();
            Albums = new LinkedList<AlbumTrack>();
        }

        public Track(string title, int duration, DateTime release)
        {
            Title = title;
            Duration = duration;
            ReleaseDate = release;
        }

        public virtual void AddArtist(Artist artist)
        {
            Artists.Add(new ArtistTrack { Track = this, Artist = artist });
        }

        public virtual void AddAlbum(Album album)
        {
            Albums.Add(new AlbumTrack { Track = this, Album = album });
        }

        public virtual void AddGenre(Genre genre)
        {
            Genres.Add(new TrackGenre { Track = this, Genre = genre });
        }

        public virtual void ChangeDetails(Track track)
        {
            Title = track.Title;
            Duration = track.Duration;
            ReleaseDate = track.ReleaseDate;
        }
    }
}