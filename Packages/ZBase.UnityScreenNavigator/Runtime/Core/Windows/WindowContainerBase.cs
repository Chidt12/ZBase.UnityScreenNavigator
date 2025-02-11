﻿using System;
using UnityEngine;
using UnityEngine.UI;
using ZBase.UnityScreenNavigator.Core.Views;
using ZBase.UnityScreenNavigator.Foundation;

namespace ZBase.UnityScreenNavigator.Core.Windows
{
    //[RequireComponent(typeof(RectMask2D), typeof(CanvasGroup))]
    public abstract class WindowContainerBase : ViewContainerBase, IWindowContainer
    {
        public string LayerName { get; private set; }

        public WindowContainerType LayerType { get; private set; }

        public IWindowContainerManager ContainerManager { get; private set; }

        public Canvas Canvas { get; private set; }

        protected WindowContainerConfig Config { get; private set; }

        public void Initialize(
              WindowContainerConfig config
            , IWindowContainerManager manager
            , UnityScreenNavigatorSettings settings
        )
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Settings = settings ? settings : throw new ArgumentNullException(nameof(settings));

            ContainerManager = manager ?? throw new ArgumentNullException(nameof(manager));
            ContainerManager.Add(this);

            LayerName = config.name;
            LayerType = config.containerType;
            
            var canvas = GetComponent<Canvas>();

            if (canvas && config.overrideSorting)
            {
                canvas.overrideSorting = true;
                canvas.sortingLayerID = config.sortingLayer.id;
                canvas.sortingOrder = config.orderInLayer;
            }

            Canvas = canvas;

            InitializePool();
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        protected override void InitializePool()
        {
            base.InitializePool();

            if (Canvas)
            {
                var poolCanvas = PoolTransform.gameObject.GetOrAddComponent<Canvas>();
                poolCanvas.overrideSorting = true;
                poolCanvas.sortingLayerID = Canvas.sortingLayerID;
                poolCanvas.sortingOrder = Canvas.sortingOrder;
            }
        }
    }
}