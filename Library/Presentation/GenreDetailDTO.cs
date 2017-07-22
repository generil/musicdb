using System;
using System.Collections.Generic;

namespace Music.Presentation
{
    public class GenreDetailDTO : BaseDTO
    {
        public string Type { get; set; }

        public GenreDetailDTO()
        {
        }

        public GenreDetailDTO(string type) : this()
        {
            Type = type;
        }
    }
}