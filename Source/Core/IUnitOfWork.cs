using System;

namespace DDDIntro.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
    }
}