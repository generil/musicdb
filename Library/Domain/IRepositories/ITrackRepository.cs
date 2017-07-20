using System;
using Music.Domain.Entities;
using Music.Infrastructure.Repositories;

namespace Music.Domain.IRepositories
{
    public interface ITrackRepository : IRepository<Track, long>
    {
    }
}