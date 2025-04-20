using Cysharp.Threading.Tasks;

namespace Source.Infrastructure.Services
{
    public interface IUIService
    {
        UniTask<T> OpenScreen<T>() where T : UIScreen;
        UniTask CloseScreen(UIScreen screen);
    }
}
