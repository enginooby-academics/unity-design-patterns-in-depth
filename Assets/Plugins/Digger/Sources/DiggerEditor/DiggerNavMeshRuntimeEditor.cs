using UnityEditor;

namespace Digger
{
    #region DiggerPRO

    [CustomEditor(typeof(DiggerMasterRuntime))]
    public class DiggerNavMeshRuntimeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This script doesn't do anything by itself. You must call its methods CollectNavMeshSources and " +
                                    "UpdateNavMeshAsync from your code. You can also change the NavMesh build settings thanks to the BuildSettings property.", MessageType.Info);
        }
    }

    #endregion
}