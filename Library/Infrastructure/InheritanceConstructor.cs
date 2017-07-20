using Music.Domain.Entities;

namespace Music.Infrastructure
{
    public class InheritanceConstructor
    {
        public static Artist ReConstructArtist(Record record, Artist artist)
        {
            artist.Id = record.Id;
            return artist;
        }
    }
}