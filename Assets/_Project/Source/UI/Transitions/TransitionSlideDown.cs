using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.Infrastructure.UI;
using Source.Infrastructure.UI.Transitions;
using UnityEngine;

namespace Source.UI.Transitions
{
    public class TransitionSlideDown : ITransition
    {
        public UniTask Play(UIScreen from, UIScreen to)
        {
            var tasks = new List<UniTask>();
            var fromHeight = from.TransitionRoot.rect.height;
            var toHeight = to.TransitionRoot.rect.height;
            if (from != null)
            {
                tasks.Add(from.TransitionRoot.DOAnchorPosY(-fromHeight, 0.3f).AsyncWaitForCompletion().AsUniTask());
            }

            if (to != null)
            {
                to.TransitionRoot.anchoredPosition = new Vector2(0, toHeight);
                tasks.Add(to.TransitionRoot.DOAnchorPosY(0, 0.3f).AsyncWaitForCompletion().AsUniTask());
            }

            return UniTask.WhenAll(tasks);
        }
    }
}