using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;
using Music.Domain.IRepositories;
using Music.Infrastructure.UnitsOfWork;

namespace Music.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MainUnitOfWork MainUnitOfWork;
        public MainUnitOfWork UnitOfWork => MainUnitOfWork;

        public GenreRepository(MainUnitOfWork unitOfWork)
        {
            MainUnitOfWork = unitOfWork;
        }

        public void Add(Genre entity)
        {
            Record record = new Record();
            MainUnitOfWork.FirstGenUnitOfWork.Records.Add(record);
            MainUnitOfWork.FirstGenUnitOfWork.Commit();

            Genre genre = new Genre(entity.Type);
            MainUnitOfWork.SecondGenUnitOfWork.Genres.Add(genre);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }

        public Genre Get(long id)
        {
            var genre = MainUnitOfWork.SecondGenUnitOfWork.Genres.Where(ge => ge.Id == id)
                .Include(tr => tr.Tracks).ThenInclude(tr => tr.Track)
                .FirstOrDefault();

            return genre;
        }

        public IEnumerable<Genre> GetAll()
        {
            var genres = MainUnitOfWork.SecondGenUnitOfWork.Genres
                .Include(tr => tr.Tracks).ThenInclude(tr => tr.Track);

            return genres;
        }

        public void Modify(Genre entity)
        {
            var record = MainUnitOfWork.FirstGenUnitOfWork.Records.FirstOrDefault(re => re.Id == entity.Id);
            MainUnitOfWork.FirstGenUnitOfWork.Entry(record).State = EntityState.Modified;
            MainUnitOfWork.FirstGenUnitOfWork.Commit();

            var genre = MainUnitOfWork.SecondGenUnitOfWork.Genres.FirstOrDefault(ge => ge.Id == entity.Id);
            genre.ChangeDetails(genre);
            MainUnitOfWork.SecondGenUnitOfWork.Entry(genre).State = EntityState.Modified;
            MainUnitOfWork.SecondGenUnitOfWork.Commit();

        }

        public void Remove(Genre entity)
        {
            var genre = MainUnitOfWork.SecondGenUnitOfWork.Genres.FirstOrDefault(p => p.Id == entity.Id);
            MainUnitOfWork.SecondGenUnitOfWork.Genres.Remove(genre);
            MainUnitOfWork.SecondGenUnitOfWork.Commit();
        }
    }
}