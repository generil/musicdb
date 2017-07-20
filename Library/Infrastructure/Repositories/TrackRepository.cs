using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;
using Music.Domain.IRepositories;
using Music.Infrastructure.UnitsOfWork;

namespace Music.Infrastructure.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly MainUnitOfWork MainUnitOfWork;
        public MainUnitOfWork UnitOfWork => MainUnitOfWork;

        public TrackRepository(MainUnitOfWork unitOfWork)
        {
            MainUnitOfWork = unitOfWork;
        }

        public void Add(Track entity)
        {
            Record record = new Record();
            MainUnitOfWork.FirstGenUnitOfWork.Records.Add(record);
            MainUnitOfWork.FirstGenUnitOfWork.Commit();

            Track track = new Track(entity.Title, entity.Duration, entity.ReleaseDate);
            MainUnitOfWork.SecondGenUnitOfWork.Tracks.Add(track);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public Track Get(long id)
        {
            var track = MainUnitOfWork.SecondGenUnitOfWork.Tracks.Where(tr => tr.Id == id)
                .Include(ar => ar.Artists).ThenInclude(ar => ar.Artist)
                .Include(al => al.Albums).ThenInclude(al => al.Album)
                .Include(ge => ge.Genres).ThenInclude(ge => ge.Genre)
                .FirstOrDefault();

            return track;
        }

        public IEnumerable<Track> GetAll()
        {
            var tracks = MainUnitOfWork.SecondGenUnitOfWork.Tracks
                .Include(ar => ar.Artists).ThenInclude(ar => ar.Artist)
                .Include(al => al.Albums).ThenInclude(al => al.Album)
                .Include(ge => ge.Genres).ThenInclude(ge => ge.Genre);

            return tracks;
        }

        public void Modify(Track entity)
        {
            var party = MainUnitOfWork.FirstGenUnitOfWork.Records.FirstOrDefault(re => re.Id == entity.Id);
            MainUnitOfWork.FirstGenUnitOfWork.Entry(party).State = EntityState.Modified;
            MainUnitOfWork.FirstGenUnitOfWork.Commit();

            var track = MainUnitOfWork.SecondGenUnitOfWork.Tracks.FirstOrDefault(tr => tr.Id == entity.Id);
            track.ChangeDetails(entity);
            MainUnitOfWork.SecondGenUnitOfWork.Entry(track).State = EntityState.Modified;
            MainUnitOfWork.SecondGenUnitOfWork.Commit();

        }

        public void Remove(Track entity)
        {
            var track = MainUnitOfWork.SecondGenUnitOfWork.Tracks.FirstOrDefault(p => p.Id == entity.Id);
            MainUnitOfWork.SecondGenUnitOfWork.Tracks.Remove(track);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }
    }
}