using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class AlbumDetailDTO
    {
        public string Title { get; set; }
        public string RecordLabel { get; set; }
        public DateTime ReleaseDate { get; set; }

        public AlbumDetailDTO()
        {
        }

        public AlbumDetailDTO(string title, string label, DateTime release) : this()
        {
            Title = title;
            RecordLabel = label;
            ReleaseDate = release;
        }
    }
}