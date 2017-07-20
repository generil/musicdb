using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;
using Music.Domain.IRepositories;
using Music.Infrastructure.UnitsOfWork;

namespace Music.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MainUnitOfWork MainUnitOfWork;
        public MainUnitOfWork UnitOfWork => MainUnitOfWork;

        public ArtistRepository(MainUnitOfWork unitOfWork)
        {
            MainUnitOfWork = unitOfWork;
        }

        public void Add(Artist entity)
        {
            Record record = new Record();
            MainUnitOfWork.FirstGenUnitOfWork.Records.Add(record);
            MainUnitOfWork.FirstGenUnitOfWork.Commit();

            // Artist artist = new Artist(entity.Name, entity.Description, entity.Country, entity.YearActive);
            entity.Id = record.Id;
            MainUnitOfWork.SecondGenUnitOfWork.Artists.Add(entity);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public Artist Get(long id)
        {
            var artist = MainUnitOfWork.SecondGenUnitOfWork.Artists.Where(ar => ar.Id == id)
                .Include(tr => tr.Tracks).ThenInclude(tr => tr.Track)
                .Include(al => al.Albums).ThenInclude(al => al.Album)
                .FirstOrDefault();

            return artist;
        }

        public IEnumerable<Artist> GetAll()
        {
            var artists = MainUnitOfWork.SecondGenUnitOfWork.Artists
                .Include(tr => tr.Tracks).ThenInclude(tr => tr.Track)
                .Include(al => al.Albums).ThenInclude(al => al.Album);

            return artists;
        }

        public void Modify(Artist entity)
        {
            var record = MainUnitOfWork.FirstGenUnitOfWork.Records.FirstOrDefault(re => re.Id == entity.Id);
            MainUnitOfWork.FirstGenUnitOfWork.Entry(record).State = EntityState.Modified;
            MainUnitOfWork.FirstGenUnitOfWork.Commit();

            var artist = MainUnitOfWork.SecondGenUnitOfWork.Artists.FirstOrDefault(ar => ar.Id == entity.Id);
            artist.ChangeDetails(entity);
            MainUnitOfWork.SecondGenUnitOfWork.Entry(artist).State = EntityState.Modified;
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public void Remove(Artist entity)
        {
            var artist = MainUnitOfWork.SecondGenUnitOfWork.Artists.FirstOrDefault(ar => ar.Id == entity.Id);
            MainUnitOfWork.SecondGenUnitOfWork.Artists.Remove(artist);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();

            var record = MainUnitOfWork.FirstGenUnitOfWork.Records.FirstOrDefault(re => re.Id == entity.Id);
            MainUnitOfWork.FirstGenUnitOfWork.Records.Remove(record);
            MainUnitOfWork.FirstGenUnitOfWork.Commit();
        }
    }
}