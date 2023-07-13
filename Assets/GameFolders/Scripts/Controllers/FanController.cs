using System;
using DG.Tweening;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.GridSystem.ExternalGrid;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Managers.HighLevelManagers;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.Controllers
{
    [Serializable]
    public class OnFanMergedEvent : UnityEvent
    {
    }

    [DefaultExecutionOrder(15000)]
    public class FanController : FanControllerBase
    {
        [SerializeField] private FanCanvasBehaviour canvas;
        [SerializeField] private ForwardType forwardType;
        [SerializeReference] private FanCanvasBehaviour canvasPrefab;
        [SerializeField] private GameObject visualizer;

        [FormerlySerializedAs("GridParent")] [SerializeField]
        private GameObject gridParent;

        public FanControllerBase MergeFan;
        public bool isActive;
        public bool isLocked;
        public bool isStatic;
        private bool _isMerging;
        private Tween _mergeTween, _colorTween;
        private MeshRenderer _meshRenderer;
        private bool _destroyCalled;

        private Vector3 colliderCenter, colliderSize;

        [SerializeField] private OnFanMergedEvent onFanMergedEvent;

        public FanController(Tween colorTween)
        {
            _colorTween = colorTween;
        }

        private ForwardType ForwardType
        {
            get => forwardType;
            set
            {
                Vector3 rotation = Vector3.zero;
                Vector3 offset = Vector3.up;
                switch (value)
                {
                    case ForwardType.Right:
                        rotation = new Vector3(0, 0, 90);
                        offset = Vector3.left * .5f;
                        break;
                    case ForwardType.Left:
                        rotation = new Vector3(0, 0, 270);
                        offset = Vector3.right * .5f;
                        break;
                    case ForwardType.Forward:
                        rotation = new Vector3(0, 0, 180);
                        offset = Vector3.back * .5f;
                        break;
                    case ForwardType.Backward:
                        rotation = new Vector3(0, 0, 0);
                        offset = Vector3.forward * .5f;
                        break;
                }

                canvas.SetPositionAndRotation(FanMovementController.Instance.GetScreenPosForCanvas(transform.position + offset), Quaternion.Euler(rotation));
                forwardType = value;
            }
        }

        private ExternalGridCell GetCurrentCell => ExternalGridController.Instance.GetNearestCell(transform.position);

        public bool IsMerging
        {
            get => _isMerging;
            set
            {
                if (value == _isMerging) return;
                _isMerging = value;
                SetMergeAnimation(value);
            }
        }

        public float GetExternalGridPos => ExternalGridController.Instance.GetExternalCellPosition();

        private void OnDestroy()
        {
            if (canvas) canvas.StartDestroy();
            OnRevert -= OnOnRevert;
        }

        protected void Start()
        {
            SaveCollider();
            if (!isStatic)
            {
                visualizer.SetActive(false);
                gridParent.SetActive(false);
                OnRevert += OnOnRevert;
            }

            canvas = Instantiate(canvasPrefab, Vector3.zero, Quaternion.identity, MainCanvasReference.Instance.transform);
            canvas.SetPositionAndRotation(GetPositionForFanCanvas(transform.position, Vector3.up), Quaternion.identity);
            _meshRenderer = GetComponent<MeshRenderer>();
            GetCurrentCell.OccupyItem(gameObject);
            UpdateText();
            UpdateForwardType();
            SetActionPoints();
        }

        private void OnOnRevert(RevertModel obj)
        {
            if (isLocked) isLocked = false;
            if (!isActive) isActive = true;
            DoMoveTweenFirstSection(obj.Position, obj.Rotation);
        }

        private void SetMergeAnimation(bool active)
        {
            if (isLocked) return;
            var cell = ExternalGridController.Instance.GetNearestCell(transform.position);
            if (active && _mergeTween == null)
            {
                if (cell.IsLockable) SetActionPointsOnMerge(Power + MergeFan.Power);
                _mergeTween = transform.DOScale(1.2f * ScaleFactor, 1f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
                canvas.SetMergeAnimation(true, Power + MergeFan.Power);
            }
            else
            {
                if (cell.IsLockable) SetActionPoints();
                _mergeTween.Complete();
                _mergeTween.Kill();
                transform.DOScale(ScaleFactor, .5f);
                _mergeTween = null;
                canvas.SetMergeAnimation(false, Power);
            }

            Vibrator.Light();
        }

        public void MergeIt(FanController from)
        {
            var cell = GetCurrentCell;
            if (cell.IsLockable)
            {
                visualizer.SetActive(true);
                gridParent.SetActive(true);
            }
            else
            {
                visualizer.SetActive(false);
                gridParent.SetActive(false);
            }

            AddRevertModel();
            onFanMergedEvent?.Invoke();
            IsMerging = false;
            if (_mergeTween != null) return;
            DoMergeTweenFirstSection();
            Power += from.Power;
            UpdateText();
            from.StartDestroy(.5f);
            if (cell.IsLockable)
            {
                SetActionPoints();
                visualizer.SetActive(true);
                gridParent.SetActive(true);
            }
            else
            {
                visualizer.SetActive(false);
                gridParent.SetActive(false);
            }
        }

        private void UpdateForwardType()
        {
            var euler = transform.eulerAngles;
            if (GameManager.Instance.GameIsVeritcal)
            {
                if (euler.x == 0)
                {
                    ForwardType = ForwardType.Right;
                }
                else if (Math.Abs(euler.x - 90) < 20f)
                {
                    ForwardType = ForwardType.Up;
                }
                else if (Math.Abs(euler.x - 180) < 20f)
                {
                    ForwardType = ForwardType.Left;
                }
                else if (Math.Abs(euler.x - 270) < 20f)
                {
                    ForwardType = ForwardType.Down;
                }
            }
            else
            {
                if (euler.y == 0)
                {
                    ForwardType = ForwardType.Forward;
                }
                else if (Math.Abs(euler.y - 90) < 20f)
                {
                    ForwardType = ForwardType.Right;
                }
                else if (Math.Abs(euler.y - 180) < 20f)
                {
                    ForwardType = ForwardType.Backward;
                }
                else if (Math.Abs(euler.y - 270) < 20f)
                {
                    ForwardType = ForwardType.Left;
                }
            }
        }

        private void UpdateText()
        {
            canvas.SetText(Power);
        }

        private void SaveCollider()
        {
            var collider = GetComponent<BoxCollider>();
            colliderSize = collider.size;
            colliderCenter = collider.center;
        }

        private void RemoveCollider()
        {
            Destroy(GetComponent<BoxCollider>());
        }

        private void AddCollider()
        {
            var collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = colliderSize;
            collider.center = colliderCenter;
        }

        public void SetPosition(ExternalGridCell cell)
        {
            if (cell.IsOccupied) return;
            var currentCell = ExternalGridController.Instance.GetNearestCell(transform.position);
            if (cell == currentCell) return;
            if (cell.IsLockable)
            {
                visualizer.SetActive(true);
                gridParent.SetActive(true);
            }
            else
            {
                visualizer.SetActive(false);
                gridParent.SetActive(false);
            }

            if (Vector3.Distance(cell.Position, transform.position) < .5f) return;
            Vibrator.Light();
            cell.OccupyItem(gameObject);
            isLocked = cell.IsLockable;
            gridParent.SetActive(false);
            visualizer.SetActive(false);
            DoMoveTweenFirstSection(cell.Position, cell.GetRotation());
        }

        private void StartDestroy(float destroyDelay)
        {
            GetCurrentCell.OccupyItem(null);
            if (_destroyCalled) return;
            _destroyCalled = true;
            canvas.StartDestroy();
            transform.DOKill();
            transform.DOScale(.01f * ScaleFactor, .5f)
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.one;
                    transform.position = Vector3.zero;
                    gameObject.SetActive(false);
                });
        }

        private void DoMoveTweenFirstSection(Vector3 position, Quaternion rotation)
        {
            RemoveCollider();
            AddRevertModel();
            GetCurrentCell.OccupyItem(null);
            transform.DOKill(true);
            transform.DOScale(.1f * ScaleFactor, .3f)
                .SetEase(Ease.InOutCirc)
                .OnComplete(() => OnFirstTweenCompleted(position, rotation));
        }

        private void OnFirstTweenCompleted(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            UpdateForwardType();
            DoMoveTweenSecondSection();
        }

        private void DoMoveTweenSecondSection()
        {
            isReverting = false;
            transform.DOScale(1f * ScaleFactor, .3f).SetEase(Ease.InOutCirc).OnComplete(AddCollider);
            Vibrator.Light();
            if (GetCurrentCell.IsLockable)
            {
                gridParent.SetActive(true);
                visualizer.SetActive(true);
                SetActionPoints();
                MoveCounter.UseMove();
            }
            else
            {
                gridParent.SetActive(false);
                visualizer.SetActive(false);
            }
        }

        private void DoMergeTweenFirstSection()
        {
            _mergeTween = transform.DOScale(1.1f * ScaleFactor, .5f).SetEase(Ease.InCirc).OnComplete(DoMergeTweenSecondSection);
        }

        private void DoMergeTweenSecondSection()
        {
            _mergeTween = transform.DOScale(1f * ScaleFactor, .5f).SetEase(Ease.OutCirc).OnComplete(() => _mergeTween = null);
        }

        private Vector3 GetPositionForFanCanvas(Vector3 refPos, Vector3 offset) => FanMovementController.Instance.GetScreenPosForCanvas(refPos + offset);

        public void Select(bool active)
        {
            if (active)
            {
                transform.DOScale(0.5f * ScaleFactor, .5f);
                if (GetCurrentCell.IsLockable)
                {
                    visualizer.SetActive(false);
                    gridParent.SetActive(false);
                }
            }
            else
            {
                transform.DOScale(1f * ScaleFactor, .5f);
                if (GetCurrentCell.IsLockable)
                {
                    visualizer.SetActive(true);
                    gridParent.SetActive(true);
                }
            }

            Vibrator.Haptic(HapticPatterns.PresetType.Selection);
        }

        public void Reset()
        {
            transform.DOKill();
            transform.localScale = Vector3.one * ScaleFactor;
            UpdateForwardType();
            GetCurrentCell.OccupyItem(null);
            isLocked = false;
            gridParent.SetActive(false);
            visualizer.SetActive(false);
            _destroyCalled = false;
        }
        
    }
}