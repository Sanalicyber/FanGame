using System;
using DG.Tweening;
using Framework.Core;
using Framework.UI;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Views;
using UnityEngine;

namespace GameFolders.Scripts.Presenters
{
    public class SuccessPresenter : BasePresenter
    {
        public GameObject SuccessPanel;

        private void Start()
        {
            ((SuccessView)view).gameObject.SetActive(true);
        }

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case OnLevelCompletedEventArgs onLevelCompletedEventArgs:
                    var canvasses = FindObjectsOfType<FanCanvasBehaviour>();
                    foreach (var canvas in canvasses)
                    {
                        canvas.StartDestroy();
                    }
                    SetActive(true);
                    break;
                case OnLevelCreatedEventArgs onLevelCreatedEventArgs:
                    SetActive(false);
                    break;
            }
        }

        public void NextLevelButtonHandler()
        {
            BroadcastUpward(new NextLevelButtonClickedEventArgs());
        }

        private void SetActive(bool active)
        {
            if (active)
            {
                SuccessPanel.SetActive(true);
                SuccessPanel.transform.localScale = Vector3.zero;
                SuccessPanel.GetComponent<CanvasGroup>().alpha = 0;
                SuccessPanel.GetComponent<CanvasGroup>().DOFade(1f, 1f);
                SuccessPanel.transform.DOScale(1f, 1f);
            }
            else
            {
                SuccessPanel.GetComponent<CanvasGroup>().DOFade(0f, .75f);
                SuccessPanel.transform.DOScale(0f, 1f).OnComplete(() =>
                {
                    SuccessPanel.SetActive(false);
                });
            }
        }
    }
}