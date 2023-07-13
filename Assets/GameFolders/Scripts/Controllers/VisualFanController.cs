using System;
using DG.Tweening;
using GameFolders.Scripts.GridSystem.ExternalGrid;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Managers.HighLevelManagers;
using UnityEditor;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class VisualFanController : FanControllerBase
    {
        public event Action<ExternalGridCell> OnFanSnapped;

        public FanController SnappedFan, MergeFan;
        public bool SnappedToMouse;
        public Camera mainCam;
        public GameObject gridContainer;

        private Vector3 _mousePositionCalculated;

        private Vector3 _mousePos => Input.mousePosition;

        private void Start()
        {
            SetActionPoints();
        }

        private void Update()
        {
            if (!SnappedToMouse) return;

            _mousePositionCalculated = mainCam.ScreenToWorldPoint(_mousePos);

            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.GameIsVeritcal)
                {
                    _mousePositionCalculated.z = transform.position.z;
                }
                else
                {
                    _mousePositionCalculated.y = transform.position.y;
                }
            }
            else
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }

            var mousePos = _mousePositionCalculated;

            var cell = ExternalGridController.Instance.GetNearestCell(mousePos);

            if (Vector3.Distance(cell.Position, transform.position) > 0.5f)
            {
                DoMove(cell);
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnFanSnapped?.Invoke(cell);

                if (MergeFan != null) MergeFan.MergeIt(SnappedFan);

                SnappedToMouse = false;
            }
        }

        private void DoMove(Vector3 snappedPos)
        {
            SetActionPoints();
            transform.DOScale(0, 0.2f)
                .SetEase(Ease.InBounce)
                .OnComplete(() =>
                {
                    transform.position = snappedPos;
                    transform.DOScale(ScaleFactor, 0.2f).SetEase(Ease.OutBounce);
                });
        }

        private void DoMove(ExternalGridCell cell)
        {
            bool cellInRange = (cell.IsLockable && !cell.IsOccupied) || cell.IsLockable;
            gridContainer.SetActive(cellInRange);
            if (cellInRange) SetActionPoints();

            Vibrator.Light();
            transform.position = cell.Position;
            transform.rotation = cell.GetRotation();
            // transform.DOScale(0, 0.1f)
            //     .SetEase(Ease.InBounce)
            //     .OnComplete(() =>
            //     {
            //         transform.DOScale(ScaleFactor, 0.1f).SetEase(Ease.OutBounce);
            //     });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Fan"))
            {
                var fan = other.GetComponent<FanController>();
                if (fan != null && fan != SnappedFan)
                {
                    if (!fan.isLocked && !fan.IsMerging && !Equals(fan, MergeFan))
                    {
                        MergeFan = fan;
                        fan.MergeFan = SnappedFan;
                        fan.IsMerging = true;
                    }
                    else
                    {
                        fan.Shake();
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Fan"))
            {
                var fan = other.GetComponent<FanController>();
                if (fan != null && fan != SnappedFan)
                {
                    if (!fan.isLocked && !fan.IsMerging && !Equals(fan, MergeFan))
                    {
                        MergeFan = fan;
                        fan.MergeFan = SnappedFan;
                        fan.IsMerging = true;
                    }
                    else
                    {
                        fan.Shake();
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Fan"))
            {
                var fan = other.GetComponent<FanController>();
                if (fan != null && fan != SnappedFan)
                {
                    if (fan.IsMerging || Equals(fan, MergeFan))
                    {
                        MergeFan.IsMerging = false;
                        MergeFan = null;
                    }
                }
            }
        }
    }
}