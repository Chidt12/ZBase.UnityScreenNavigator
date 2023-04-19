﻿using System;

namespace ZBase.UnityScreenNavigator.Core.Views
{
    public delegate void OnLoadCallback(Window window, Memory<object> args);

    public readonly struct WindowOptions
    {
        public readonly bool loadAsync;
        public readonly bool playAnimation;
        public readonly bool ignorePoolingSetting;
        public readonly string resourcePath;
        public readonly OnLoadCallback onLoaded;

        public WindowOptions(
            string resourcePath
            , bool playAnimation = true
            , OnLoadCallback onLoaded = null
            , bool loadAsync = true
            , bool ignorePoolingSetting = false
        )
        {
            this.loadAsync = loadAsync;
            this.playAnimation = playAnimation;
            this.ignorePoolingSetting = ignorePoolingSetting;
            this.resourcePath = resourcePath;
            this.onLoaded = onLoaded;
        }

        public static implicit operator WindowOptions(string resourcePath)
            => new(resourcePath);
    }
}