using System;

namespace DDDIntro.Core
{
    public interface IUnitOfWork : IAggregateRepository, IDisposable
    {
        void Complete();
    }
}