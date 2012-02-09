using System;

namespace DDDIntro.Core
{
    public interface IUnitOfWork : IUniversalRepository, IDisposable
    {
        void Complete();
    }
}