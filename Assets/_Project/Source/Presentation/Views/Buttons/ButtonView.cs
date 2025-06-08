using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Yans.UI.Views;

namespace Source.Presentation.Views
{
    public class ButtonView : View
    {
        [SerializeField] private Button _clickInterceptor;
        [SerializeField] private CanvasGroup _canvasGroup;

        public bool Interactable
        {
            get => _clickInterceptor.interactable;
            set => _clickInterceptor.interactable = value;
        }

        public delegate void ClickOnViewDelegate(View view);

        private event ClickOnViewDelegate OnClickOnViewInternal;

        public event ClickOnViewDelegate OnClick
        {
            add
            {
                OnClickOnViewInternal += value;
                UpdateClickInterceptor();
            }
            remove
            {
                OnClickOnViewInternal -= value;
                UpdateClickInterceptor();
            }
        }

        public void PerformPointerClick()
        {
            OnClickOnViewInternal?.Invoke(this);
        }


        private void UpdateClickInterceptor()
        {
            if (_clickInterceptor != null)
                _clickInterceptor.enabled = OnClickOnViewInternal != null &&
                              OnClickOnViewInternal.GetInvocationList().Length > 0;
        }

        protected override void Awake()
        {
            base.Awake();

            if (_clickInterceptor != null)
                _clickInterceptor.enabled = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_clickInterceptor != null)
            {
                _clickInterceptor.onClick.AddListener(PerformPointerClick);
                _clickInterceptor.enabled = OnClickOnViewInternal?.GetInvocationList().Length > 0;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_clickInterceptor != null)
            {
                _clickInterceptor.onClick.RemoveListener(PerformPointerClick);
            }
        }

        protected override void OnDestroy()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.DOKill();
            }
        }

        protected override void SetViewVisible()
        {
            if (_canvasGroup != null)
            {
                base.SetViewVisible();

                _canvasGroup.DOKill();
                _canvasGroup.alpha = 0f;
                _canvasGroup.DOFade(1f, 0.2f)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() =>
                    {
                        _canvasGroup.interactable = true;
                    });
            }
        }

        protected override void SetViewHidden()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = false;

                _canvasGroup.DOKill();
                _canvasGroup.DOFade(0f, 0.2f)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() =>
                    {
                        base.SetViewHidden();
                    });
            }
        }
    }
}