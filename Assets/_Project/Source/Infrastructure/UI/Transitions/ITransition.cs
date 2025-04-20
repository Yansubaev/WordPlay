using Cysharp.Threading.Tasks;

namespace Source.Infrastructure.UI.Transitions
{
    public interface ITransition
    {
        public UniTask Play(UIScreen from, UIScreen to);
    }
}
