using Cysharp.Threading.Tasks;
using Yans.UI.Screen;

namespace Yans.UI
{
    public interface IScreenManager
    {
        UniTask<T> OpenPanel<T>() where T : UIPanel;
        UniTask ClosePanel(UIPanel screen);
        UniTask<T> OpenPopup<T>() where T : UIPopup;
        UniTask ClosePopup(UIPopup popup);
        UniTask CloseTop();
    }
}
