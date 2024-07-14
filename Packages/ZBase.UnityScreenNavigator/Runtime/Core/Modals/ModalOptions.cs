using ZBase.UnityScreenNavigator.Core.Views;

namespace ZBase.UnityScreenNavigator.Core.Modals
{
    public readonly struct ModalOptions
    {
        public readonly string modalBackdropResourcePath;
        public readonly ViewOptions options;

        public ModalOptions(
              in ViewOptions options
            , string modalBackdropResourcePath = null
        )
        {
            this.options = options;
            this.modalBackdropResourcePath = modalBackdropResourcePath;
        }

        public ModalOptions(
              string resourcePath
            , bool playAnimation = true
            , OnViewLoadedCallback onLoaded = null
            , bool loadAsync = true
            , string modalBackdropResourcePath = null
            , PoolingPolicy poolingPolicy = PoolingPolicy.UseSettings
        )
        {
            this.options = new(resourcePath, playAnimation, onLoaded, loadAsync, poolingPolicy);
            this.modalBackdropResourcePath = modalBackdropResourcePath;
        }

        public static implicit operator ModalOptions(in ViewOptions options)
            => new(options);

        public static implicit operator ModalOptions(string resourcePath)
            => new(new ViewOptions(resourcePath));

        public static implicit operator ViewOptions(in ModalOptions options)
            => options.options;
    }
}
