using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

#if UNITY_EDITOR
#endif

[ExecuteInEditMode]
public class ModelSwitchManager : MonoBehaviour {
  [Range(0, 2)] [SerializeField] private int globalActiveModelIndex;

  [InlineEditor] [Space] [SerializeField]
  private List<ModelSwitchEditor> prefabs;

  // Start is called before the first frame update
  private void Start() {
  }

  // Update is called once per frame
  private void Update() {
  }

  private void OnValidate() {
    foreach (var prefab in prefabs) prefab.SetActiveModelIndex(globalActiveModelIndex);

    // TODO: auto save scene after make change on this manager
    // EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
  }
}