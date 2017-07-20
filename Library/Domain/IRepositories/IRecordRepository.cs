using System;
using Music.Infrastructure.UnitsOfWork;

namespace Music.Domain.IRepositories
{
    public interface IRecordRepository
    {
        MainUnitOfWork UnitOfWork { get; }
    }
}