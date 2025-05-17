using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Yans.UI.UIScreens;
using Yans.UI.Transitions;

namespace Source.UI.Transitions
{
    public class TransitionFade : ITransition
    {
        public UniTask Play(UIPanel from, UIPanel to)
        {
            var tasks = new List<UniTask>();

            if (from != null)
            {
                tasks.Add(from.FadeRoot.DOFade(0, 0.3f).AsyncWaitForCompletion().AsUniTask());
            }

            if (to != null)
            {
                to.FadeRoot.alpha = 0;
                tasks.Add(to.FadeRoot.DOFade(1, 0.4f).AsyncWaitForCompletion().AsUniTask());
            }

            return UniTask.WhenAll(tasks);
        }
    }
}
