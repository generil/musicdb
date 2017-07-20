using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;
using Music.Domain.IRepositories;
using Music.Infrastructure.UnitsOfWork;

namespace Music.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MainUnitOfWork MainUnitOfWork;
        public MainUnitOfWork UnitOfWork => MainUnitOfWork;

        public AlbumRepository(MainUnitOfWork unitOfWork)
        {
            MainUnitOfWork = unitOfWork;
        }

        public void Add(Album entity)
        {
            MainUnitOfWork.SecondGenUnitOfWork.Albums.Add(entity);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public Album Get(long id)
        {
            var artist = MainUnitOfWork.SecondGenUnitOfWork.Albums.Where(al => al.Id == id)
                .Include(ar => ar.Artists).ThenInclude(ar => ar.Artist)
                .Include(tr => tr.Tracks).ThenInclude(tr => tr.Track)
                .FirstOrDefault();

            return artist;
        }

        public IEnumerable<Album> GetAll()
        {
            var albums = MainUnitOfWork.SecondGenUnitOfWork.Albums
                .Include(ar => ar.Artists).ThenInclude(ar => ar.Artist)
                .Include(tr => tr.Tracks).ThenInclude(tr => tr.Track);

            return albums;
        }

        public void Modify(Album entity)
        {
            var record = MainUnitOfWork.FirstGenUnitOfWork.Records.FirstOrDefault(re => re.Id == entity.Id);
            MainUnitOfWork.FirstGenUnitOfWork.Entry(record).State = EntityState.Modified;
            MainUnitOfWork.FirstGenUnitOfWork.Commit();

            var album = MainUnitOfWork.SecondGenUnitOfWork.Albums.FirstOrDefault(al => al.Id == entity.Id);
            album.ChangeDetails(entity);
            MainUnitOfWork.SecondGenUnitOfWork.Entry(album).State = EntityState.Modified;
            MainUnitOfWork.SecondGenUnitOfWork.Commit();

        }

        public void Remove(Album entity)
        {
            var album = MainUnitOfWork.SecondGenUnitOfWork.Albums.FirstOrDefault(al => al.Id == entity.Id);
            MainUnitOfWork.SecondGenUnitOfWork.Albums.Remove(album);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }
    }
}