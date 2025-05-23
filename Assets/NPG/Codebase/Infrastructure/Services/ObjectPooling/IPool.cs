namespace NPG.Codebase.Infrastructure.Services.ObjectPooling
{
    public interface IPool<T>
    {
        T Pull();
        void Push(T obj);
    }
}