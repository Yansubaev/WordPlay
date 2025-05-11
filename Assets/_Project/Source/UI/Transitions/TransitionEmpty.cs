using Cysharp.Threading.Tasks;
using Yans.UI.Screen;
using Yans.UI.Transitions;

namespace Source.UI.Transitions
{
    public class TransitionEmpty : ITransition
    {
        public UniTask Play(UIPanel from, UIPanel to)
        {
            return UniTask.CompletedTask;
        }
    }
}
