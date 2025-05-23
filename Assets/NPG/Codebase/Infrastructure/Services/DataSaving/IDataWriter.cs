using NPG.Codebase.Infrastructure.JsonData;

namespace NPG.Codebase.Infrastructure.Services.DataSaving
{
    public interface IDataWriter : IDataReader
    {
        void Save(ref GameData data);
    }
}