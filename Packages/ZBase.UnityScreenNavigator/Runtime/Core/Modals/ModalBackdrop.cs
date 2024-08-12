using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ZBase.UnityScreenNavigator.Core.Views;
using ZBase.UnityScreenNavigator.Foundation;

namespace ZBase.UnityScreenNavigator.Core.Modals
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ModalBackdrop : View
    {
        [SerializeField] private ModalBackdropTransitionAnimationContainer _animationContainer;
        [SerializeField] private bool _closeModalWhenClicked;

        private Image _image;
        private float _originalAlpha;
        protected Modal ownerModal;

        public ModalBackdropTransitionAnimationContainer AnimationContainer => _animationContainer;

        protected override void Awake()
        {
            _image = GetComponentInChildren<Image>();
            _originalAlpha = _image ? _image.color.a : 1f;
        }

        public void Setup(RectTransform parent)
        {
            Parent = parent;
            RectTransform.FillParent(Parent);
            CanvasGroup.interactable = _closeModalWhenClicked;
            gameObject.SetActive(false);
        }

        public void SetOwnerModal(Modal ownerModal)
        {
            this.ownerModal = ownerModal;
            SetAlpha();
            SetClickEvent();
        }

        private void SetAlpha()
        {
            var image = _image;

            if (image == false)
                return;

            var alpha = _originalAlpha;

            if (ownerModal.DisableBackdropAlpha)
                alpha = 0.0f;

            var color = image.color;
            color.a = alpha;
            image.color = color;
        }

        private void SetClickEvent()
        {
            if (ownerModal.DisableBackdropClickable)
            {
                if (TryGetComponent<Button>(out var clickButton))
                    clickButton.onClick.RemoveAllListeners();
            }
            else
            {
                if (!TryGetComponent<Button>(out var button))
                {
                    button = gameObject.AddComponent<Button>();
                    button.transition = Selectable.Transition.None;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(OnClickBackdrop);
                }
            }
        }

        private void OnClickBackdrop()
        {
            if (ownerModal.CanCloseAsClickOnBackdrop)
                CallClickBackdropEvent();
        }

        protected virtual void CallClickBackdropEvent()
        {
            var modalContainer = ModalContainer.Of(transform);

            if (modalContainer.IsInTransition)
                return;

            modalContainer.Pop(true);
        }

        internal async UniTask EnterAsync(bool playAnimation)
        {
            gameObject.SetActive(true);
            RectTransform.FillParent(Parent);
            CanvasGroup.alpha = 1f;

            if (playAnimation)
            {
                var anim = GetAnimation(true);
                anim.Setup(RectTransform);
                
                await anim.PlayAsync();
            }

            RectTransform.FillParent(Parent);
        }

        internal async UniTask ExitAsync(bool playAnimation)
        {
            gameObject.SetActive(true);
            RectTransform.FillParent(Parent);
            CanvasGroup.alpha = 1f;

            if (playAnimation)
            {
                var anim = GetAnimation(false);
                anim.Setup(RectTransform);

                await anim.PlayAsync();
            }

            CanvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        private ITransitionAnimation GetAnimation(bool enter)
        {
            var anim = _animationContainer.GetAnimation(enter);

            if (anim == null)
            {
                return Settings.GetDefaultModalBackdropTransitionAnimation(enter);
            }

            return anim;
        }
    }
}