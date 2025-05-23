using Cysharp.Threading.Tasks;

namespace NPG.Codebase.Infrastructure.Services
{
    public interface ILoadingCurtain
    {
        void Show(UniTask asyncLoad);

        void Hide();
    }
}