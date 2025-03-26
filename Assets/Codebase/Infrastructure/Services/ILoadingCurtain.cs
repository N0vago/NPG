using Cysharp.Threading.Tasks;

namespace Codebase.Infrastructure.Services
{
    public interface ILoadingCurtain
    {
        void Show(UniTask asyncLoad);

        void Hide();
    }
}