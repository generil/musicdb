using System;
using Music.Domain.Entities;
using Music.Infrastructure.Repositories;

namespace Music.Domain.IRepositories
{
    public interface IArtistRepository : IRepository<Artist, long>
    {
    }
}