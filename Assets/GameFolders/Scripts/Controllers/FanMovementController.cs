using DG.Tweening;
using GameFolders.Scripts.GridSystem.ExternalGrid;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class FanMovementController : Singleton<FanMovementController>
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Camera mouseCam;

        public Camera MainCam;

        private Vector3 MousePosition => Input.mousePosition;
        private GameObject _movable;

        private void Update()
        {
            if (prefab == null) return;
            if (Input.GetMouseButtonDown(0))
            {
                var ray = mouseCam.ScreenPointToRay(MousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    var fan = hit.transform.GetComponent<FanController>();

                    if (fan == null) return;

                    if (fan.isLocked)
                    {
                        if (fan.isShaking) return;
                        fan.isShaking = true;
                        fan.transform.DOShakeRotation(0.5f, 0.1f, 10, 90, false);
                        fan.transform.DOShakeScale(0.5f, 0.1f, 10, 90, false).OnComplete(() => fan.isShaking = false);
                        return;
                    }

                    _movable = Instantiate(prefab, fan.transform.position, fan.transform.rotation);

                    if (_movable == null) return;

                    _movable.transform.SetParent(fan.transform.parent);
                    var visual = _movable.GetComponent<VisualFanController>();
                    fan.isActive = false;
                    fan.Select(true);
                    visual.Power = fan.Power;
                    visual.SnappedToMouse = true;
                    visual.SnappedFan = fan;
                    visual.mainCam = mouseCam;
                    visual.OnFanSnapped += OnFanSnapped;
                }
            }
        }

        private void OnFanSnapped(ExternalGridCell cell)
        {
            var visual = _movable.GetComponent<VisualFanController>();
            visual.SnappedFan.isActive = true;
            visual.SnappedFan.SetPosition(cell);
            visual.SnappedFan.Select(false);
            visual.OnFanSnapped -= OnFanSnapped;
            Destroy(_movable);
            _movable = null;
        }

        public Vector3 GetScreenPosForCanvas(Vector3 worldPos)
        {
            var screenPos = MainCam.WorldToScreenPoint(worldPos);
            screenPos.z = 0;
            return screenPos;
        }
    }
}