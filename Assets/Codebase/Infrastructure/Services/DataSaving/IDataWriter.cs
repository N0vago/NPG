using Codebase.Infrastructure.Data;

namespace Codebase.Infrastructure.Services.DataSaving
{
    public interface IDataWriter : IDataReader
    {
        void Save(ref GameData data);
    }
}