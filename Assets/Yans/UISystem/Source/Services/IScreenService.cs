using Cysharp.Threading.Tasks;
using Yans.UI.Screen;

namespace Yans.UI
{
    public interface IScreenService
    {
        UniTask<T> OpenScreen<T>() where T : UIPanel;
        UniTask CloseScreen(UIPanel screen);
        UniTask<T> OpenPopup<T>() where T : UIPopup;
        UniTask ClosePopup(UIPopup popup);
        UniTask CloseTop();
    }
}
