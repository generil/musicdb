using System;
using System.Collections.Generic;
using Music.Domain.Associates;
using Newtonsoft.Json;

namespace Music.Domain.Entities
{
    public class Album : Entity
    {
        public ICollection<ArtistAlbum> Artists { get; set; }
        public ICollection<AlbumTrack> Tracks { get; set; }
        public string Title { get; set; }
        public string RecordLabel { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Album()
        {
            Artists = new LinkedList<ArtistAlbum>();
            Tracks = new LinkedList<AlbumTrack>();
        }

        public Album(string title, string label, DateTime release)
        {
            Title = title;
            RecordLabel = label;
            ReleaseDate = release;
        }

        public virtual void AddTrack(Track track)
        {
            Tracks.Add(new AlbumTrack { Album = this, Track = track });
        }

        public virtual void AddArtist(Artist artist)
        {
            Artists.Add(new ArtistAlbum { Album = this, Artist = artist });
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public virtual void ChangeDetails(Album album)
        {
            Title = album.Title;
            RecordLabel = album.RecordLabel;
            ReleaseDate = album.ReleaseDate;
        }
    }
}