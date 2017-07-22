using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class TrackDetailDTO
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public TrackDetailDTO()
        {
        }

        public TrackDetailDTO(string title, int duration, DateTime release) : this()
        {
            Title = title;
            Duration = duration;
            ReleaseDate = release;
        }
    }
}