using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Music.Infrastructure.UnitsOfWork
{
    public class MainUnitOfWork : DbContext, IUnitOfWork
    {
        public FirstGenUnitOfWork FirstGenUnitOfWork { get; private set; }
        public SecondGenUnitOfWork SecondGenUnitOfWork { get; private set; }

        public MainUnitOfWork(FirstGenUnitOfWork firstGenUnitOfWork, SecondGenUnitOfWork secondUnitOfWork)
        {
            FirstGenUnitOfWork = firstGenUnitOfWork;
            SecondGenUnitOfWork = secondUnitOfWork;
        }

        public void Commit()
        {
            SaveChanges();
        }

        public void Rollback()
        {
            Dispose();
        }

        public IDbContextTransaction BeginTransaction(int gen)
        {
            IDbContextTransaction transaction = null;

            switch (gen)
            {
                case 1:
                    transaction = FirstGenUnitOfWork.Database.BeginTransaction();
                    break;
                case 2:
                    transaction = FirstGenUnitOfWork.Database.BeginTransaction();
                    SecondGenUnitOfWork.Database.UseTransaction(transaction.GetDbTransaction());
                    break;
            }

            return transaction;
        }
    }
}