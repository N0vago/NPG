using NPG.Codebase.Game.Gameplay.UI.Root;

namespace NPG.Codebase.Game.Gameplay.UI.Windows
{
    public class WindowBinder : Binder<WindowViewModel>
    {
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}