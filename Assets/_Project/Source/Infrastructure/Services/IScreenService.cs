using Cysharp.Threading.Tasks;
using Source.Infrastructure.UI;

namespace Source.Infrastructure.Services
{
    public interface IScreenService
    {
        UniTask<T> OpenScreen<T>() where T : UIScreen;
        UniTask CloseScreen(UIScreen screen);
        UniTask<T> OpenPopup<T>() where T : UIPopup;
        UniTask ClosePopup(UIPopup popup);
        UniTask CloseTop();
    }
}
