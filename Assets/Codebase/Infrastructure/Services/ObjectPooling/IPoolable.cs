using System;

namespace Codebase.Infrastructure.Services.ObjectPooling
{
    public interface IPoolable<T>
    {
        void Initialize(Action<T> returnAction);
        void ReturnToPool();
    }
}