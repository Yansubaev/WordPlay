using Cysharp.Threading.Tasks;
using Yans.UI.Screen;

namespace Yans.UI.Transitions
{
    public interface ITransition
    {
        public UniTask Play(UIPanel from, UIPanel to);
    }
}
