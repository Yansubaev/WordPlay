using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Infrastructure.UI;
using Source.Infrastructure.UI.Transitions;
using UnityEngine;

namespace Source.UI.Transitions
{
    public class TransitionSlideRight : ITransition
    {
        public UniTask Play(UIScreen from, UIScreen to)
        {
            var tasks = new List<UniTask>();
            var fromWidth = from.TransitionRoot.rect.width;
            var toWidth = to.TransitionRoot.rect.width;
            if (from != null)
            {
                tasks.Add(from.TransitionRoot.DOAnchorPosX(fromWidth, 0.3f).AsyncWaitForCompletion().AsUniTask());
            }

            if (to != null)
            {
                to.TransitionRoot.anchoredPosition = new Vector2(-toWidth, 0);
                tasks.Add(to.TransitionRoot.DOAnchorPosX(0, 0.3f).AsyncWaitForCompletion().AsUniTask());
            }

            return UniTask.WhenAll(tasks);

        }
    }
}
