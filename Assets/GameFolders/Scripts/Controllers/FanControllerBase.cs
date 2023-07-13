using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class FanControllerBase : Revertable
    {
        public List<Transform> actionPoints;
        public int Power;
        public const int MaxPower = 4;
        public bool isShaking;

        public float ScaleFactor;

        public virtual void Shake()
        {
            if (isShaking) return;
            isShaking = true;
            transform.DOShakeRotation(0.5f, 0.1f, 10, 90, false).OnComplete(() => isShaking = false);
            // transform.DOShakeScale(0.5f, 0.1f, 10, 90, false);
        }

        protected void SetActionPoints()
        {
            MainThread.Instance.RunOnMainThread(SetActionPointsRoutine());
        }

        protected void SetActionPointsOnMerge(int power)
        {
            MainThread.Instance.RunOnMainThread(SetActionPointsOnMergeRoutine(power));
        }

        private IEnumerator SetActionPointsOnMergeRoutine(int power)
        {
            int index = 0;
            foreach (var item in actionPoints)
            {
                var index1 = index;
                if (item.localScale.x != 0)
                {
                    if (index1 < power)
                    {
                        item.DOScale(0, .1f).OnComplete(() => { item.DOScale(.75f * ScaleFactor, .5f); });
                    }
                    else
                    {
                        item.DOScale(0, .1f);
                    }
                }
                else
                {
                    if (index1 < power) item.DOScale(.75f * ScaleFactor, .5f);
                }

                yield return new WaitForSeconds(.1f);
                index++;
            }
        }

        private IEnumerator SetActionPointsRoutine()
        {
            int index = 0;
            actionPoints.ForEach(x => x.localScale = Vector3.zero);
            foreach (var item in actionPoints)
            {
                if (item == null) continue;
                item.DOKill();

                if (index < Power)
                {
                    item.DOScale(.75f * ScaleFactor, .5f);
                }

                yield return new WaitForSeconds(.1f);
                index++;
            }
        }
    }
}