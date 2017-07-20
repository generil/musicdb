using System;
using System.Collections.Generic;
using Music.Domain.Associates;

namespace Music.Domain.Entities
{
    public class Genre : Entity
    {
        public ICollection<TrackGenre> Tracks { get; set; }
        public string Type { get; set; }

        public Genre()
        {
            Tracks = new LinkedList<TrackGenre>();
        }

        public Genre(string type)
        {
            Type = type;
        }

        public virtual void AddTrack(Track track)
        {
            Tracks.Add(new TrackGenre { Genre = this, Track = track });
        }

        public virtual void ChangeDetails(Genre genre)
        {
            Type = genre.Type;
        }
    }
}