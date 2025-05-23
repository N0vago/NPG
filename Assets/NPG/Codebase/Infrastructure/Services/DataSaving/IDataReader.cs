using NPG.Codebase.Infrastructure.JsonData;

namespace NPG.Codebase.Infrastructure.Services.DataSaving
{
    public interface IDataReader
    {
        void Load(GameData data);
    }
}