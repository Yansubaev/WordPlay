using Cysharp.Threading.Tasks;
using Yans.UI.UIScreens;

namespace Yans.UI
{
    public interface IScreenManager
    {
        UniTask<T> OpenScreen<T>() where T : UIScreen;
        UniTask CloseScreen(UIScreen screen);
        UniTask CloseTop();
    }
}