using Codebase.Infrastructure.Data;

namespace Codebase.Infrastructure.Services.DataSaving
{
    public interface IDataReader
    {
        void Load(GameData data);
    }
}