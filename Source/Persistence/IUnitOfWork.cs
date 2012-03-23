using System;

namespace DDDIntro.Persistence
{
    public interface IUnitOfWork : IUniversalRepository, IDisposable
    {
        void Complete();
    }
}