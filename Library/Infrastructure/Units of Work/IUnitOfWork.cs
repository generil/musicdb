using System;

namespace Music.Infrastructure.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}