using System;

namespace Music.Domain.Entities
{
    public class Record : Entity
    {
        public uint ConcurrencyStamp { get; set; }
        public Record()
        {

        }
    }
}