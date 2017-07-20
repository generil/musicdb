using System;
using System.Collections.Generic;
using Music.Domain.Associates;

namespace Music.Domain.Entities
{
    public class Artist : Record
    {
        public ICollection<ArtistTrack> Tracks { get; set; }
        public ICollection<ArtistAlbum> Albums { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public int YearActive { get; set; }

        public Artist()
        {
            Tracks = new LinkedList<ArtistTrack>();
            Albums = new LinkedList<ArtistAlbum>();
        }

        public Artist(string name, string description, string country, int year)
        {
            Name = name;
            Description = description;
            Country = country;
            YearActive = year;
        }

        public virtual void AddTrack(Track track)
        {
            Tracks.Add(new ArtistTrack { Artist = this, Track = track });
        }

        public virtual void AddAlbum(Album album)
        {
            Albums.Add(new ArtistAlbum { Artist = this, Album = album });
        }

        public virtual Track CreateTrack(string title, int duration, DateTime release)
        {
            return new Track(title, duration, release);
        }

        public virtual Album CreateAlbum(string title, string label, DateTime release)
        {
            return new Album(title, label, release);
        }

        public virtual void ChangeDetails(Artist artist)
        {
            Name = artist.Name;
            Description = artist.Description;
            Country = artist.Country;
            YearActive = artist.YearActive;
        }
    }
}