using DG.Tweening;
using GameFolders.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class FanCanvasBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation;

        private Tween _mergeTween;

        public void SetPositionAndRotation(Vector3 position, Quaternion euler)
        {
            transform.DOScale(0.1f, .1f)
                .OnComplete(() =>
                {
                    _position = position;
                    _rotation = euler;
                    transform.position = _position;
                    transform.rotation = _rotation;
                    transform.DOScale(1, .3f);
                });
        }

        public void SetMergeAnimation(bool active, int power)
        {
            if (!isActiveAndEnabled) return;
            if (active && _mergeTween == null)
            {
                _mergeTween = transform.DOScale(1.5f, 1f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
                SetText(power);
            }
            else
            {
                _mergeTween.Complete();
                _mergeTween.Kill();
                _mergeTween = null;
                transform.DOScale(1, .5f);
                SetText(power);
            }
        }

        public void SetText(int count)
        {
            countText.text = count.ToString();
        }

        public void StartDestroy()
        {
            transform.DOScale(0, 0.1f).OnComplete(() => Destroy(gameObject));
        }
    }
}