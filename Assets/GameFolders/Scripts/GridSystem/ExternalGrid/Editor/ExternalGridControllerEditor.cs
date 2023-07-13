using GameFolders.Scripts.Enums;
using UnityEditor;
using UnityEngine;

namespace GameFolders.Scripts.GridSystem.ExternalGrid.Editor
{
    [CustomEditor(typeof(ExternalGridController))]
    public class ExternalGridControllerEditor : UnityEditor.Editor
    {
        Vector3 pos = Vector3.zero;
        int minCount = 0;
        int maxCount = 0;
        ForwardType _forwardType = ForwardType.Forward;
        Vector3 _newPosition = Vector3.zero;
        bool _locked = false;

        public override void OnInspectorGUI()
        {
            ExternalGridController controller = (ExternalGridController)target;
            var serializeObject = new SerializedObject((ExternalGridController)target);

            pos = EditorGUILayout.Vector3Field("Default Position", pos);

            if (GUILayout.Button("AddPoint"))
            {
                if (controller.GetLastCellID() == 0)
                {
                    controller.AddExternalGridCell(new ExternalGridCell(controller.GetLastCellID() + 1, pos));
                }
                else
                {
                    controller.AddExternalGridCell(new ExternalGridCell(pos));
                }

                serializeObject.ApplyModifiedProperties();
                Undo.RecordObject(controller, "Add Point to Controller");
                EditorUtility.SetDirty(controller);
            }

            if (GUILayout.Button("RemovePoint"))
            {
                controller.RemoveExternalGridCell(controller.GetLastCellID());
                serializeObject.ApplyModifiedProperties();
                Undo.RecordObject(controller, "Remove Point from Controller");
                EditorUtility.SetDirty(controller);
            }

            var property = serializeObject.FindProperty("externalGridCells");
            serializeObject.Update();
            EditorGUILayout.HelpBox("External Grid Cells uses index starts to 1. Please take care of.",
                MessageType.Info);
            EditorGUILayout.PropertyField(property, true);
            serializeObject.ApplyModifiedProperties();

            minCount = EditorGUILayout.IntField("Min Count", minCount);
            maxCount = EditorGUILayout.IntField("Max Count", maxCount);
            _forwardType = (ForwardType)EditorGUILayout.EnumPopup("Forward Type", _forwardType);
            if (GUILayout.Button("Generate Forwards"))
            {
                controller.SetCellForwardForIndexes(minCount, maxCount, _forwardType);
            }

            minCount = EditorGUILayout.IntField("Min Count", minCount);
            maxCount = EditorGUILayout.IntField("Max Count", maxCount);
            _newPosition = EditorGUILayout.Vector3Field("New Position", _newPosition);
            if (GUILayout.Button("Generate Positions Z Axis"))
            {
                controller.SetCellPositionZAxisForIndexes(minCount, maxCount, _newPosition);
            }

            minCount = EditorGUILayout.IntField("Min Count", minCount);
            maxCount = EditorGUILayout.IntField("Max Count", maxCount);
            _locked = EditorGUILayout.Toggle("Locked", _locked);
            if (GUILayout.Button("Generate Locked"))
            {
                controller.SetCellLockedForIndexes(minCount, maxCount, _locked);
            }
        }

        private void OnSceneGUI()
        {
            var serializeObject = new SerializedObject((ExternalGridController)target);
            serializeObject.Update();
            ExternalGridController controller = (ExternalGridController)target;
            var cells = controller.GetCells();

            for (var i = 0; i < cells.Count; i++)
            {
                var position = cells[i].Position;
                cells[i].Position = Handles.DoPositionHandle(cells[i].Position, Quaternion.identity);
                Handles.DrawWireCube(cells[i].Position, Vector3.one);
                Handles.Label(cells[i].Position, cells[i].Id.ToString(), new GUIStyle() { fontSize = 25 });
                Handles.Label(cells[i].Position + Vector3.up, cells[i].IsLockable.ToString(), new GUIStyle() { fontSize = 10 });
                if (position != cells[i].Position)
                {
                    Undo.RecordObject(controller, "Move position of Cell");
                }

                pos = cells[i].Position + Vector3.up;
            }

            EditorUtility.SetDirty(controller);

            controller.SetCellList(cells);
            serializeObject.ApplyModifiedProperties();
        }
    }
}