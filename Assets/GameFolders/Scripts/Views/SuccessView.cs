using System;
using Framework.UI;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Views
{
    public class SuccessView : BaseView
    {
        [SerializeField] private Button nextLevelButton;

        protected override void Awake() { }

        protected override void Initialize()
        {
            nextLevelButton.onClick.SetListener(((SuccessPresenter)_presenter).NextLevelButtonHandler);
        }
    }
}