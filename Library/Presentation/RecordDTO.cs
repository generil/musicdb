using System;

namespace Music.Presentation
{
    public class RecordDTO : BaseDTO
    {
        public uint RowVersion { get; set; }

        public RecordDTO()
        {
        }
    }
}