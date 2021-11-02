using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class ModelSwitchManager : MonoBehaviour {
  [Range(0, 2)] [SerializeField] int globalActiveModelIndex = 0;

  [Space]
  [InlineEditor(InlineEditorModes.GUIOnly)]
  [SerializeField] List<ModelSwitchEditor> prefabs;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  private void OnValidate() {
    foreach (ModelSwitchEditor prefab in prefabs) {
      prefab.SetActiveModelIndex(globalActiveModelIndex);
    }

    // TODO: auto save scene after make change on this manager
    // EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
  }
}
