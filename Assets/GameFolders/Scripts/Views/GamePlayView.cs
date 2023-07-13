using Framework.UI;
using GameFolders.Scripts.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameFolders.Scripts.Views
{
    public class GamePlayView : BaseView
    {
        #region Public Variables

        public TextMeshProUGUI currentShowingLevelText;
        public TextMeshProUGUI currentMoneyText;
        
        #endregion

        #region Private Variables

        [SerializeField] private Button settingsButton;

        [FormerlySerializedAs("restartLevelButton")] [SerializeField]
        private Button revertButton;

        #endregion

        #region Implemented Methods

        protected override void Initialize()
        {
            // settingsButton.onClick.AddListener((_presenter as GamePlayPresenter).OnSettingsButtonClickHandler);
            revertButton.onClick.AddListener(RevertButtonClicked);
        }

        private void RevertButtonClicked()
        {
            ((GamePlayPresenter)_presenter).OnRevertButtonClickHandler();
            revertButton.interactable = false;
        }

        #endregion

        private float localTime, maxTime = 1f;
        private bool buttonCanPress = true;

        private void Update()
        {
            if (revertButton.interactable) return;
            localTime += Time.deltaTime;
            if (localTime >= maxTime)
            {
                localTime = 0;
                revertButton.interactable = true;
            }
        }
    }
}