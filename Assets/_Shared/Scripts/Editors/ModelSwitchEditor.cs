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
public class ModelSwitchEditor : MonoBehaviour {
  [SerializeField] private List<GameObject> models = new List<GameObject>();

  [Space] [ValueDropdown("models")] public GameObject activeModel;

  private int activeModelIndex;

  // Start is called before the first frame update
  private void Start() {
    // Undo.RecordObject(gameObject, "descriptive name of this operation");
    // Undo.RecordObject(gameObject.GetComponent<ModelSwitchEditor>(), "Switch model");
    // activeModel = models[0];
  }

  // Update is called once per frame
  private void Update() {
  }

  private void OnValidate() {
    // for (int i = 0; i < models.Count; i++)
    // {
    //     models[i].SetActive(i == activeModelIndex);
    // }

    foreach (var model in models) model.SetActive(model == activeModel);

    // TODO: Apply prefab when make changes on instances
    // UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    // PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.UserAction); // freeze editor!
  }

  public void SetActiveModelIndex(int index) {
    if (index > models.Count - 1) {
      print("Model index not exist");
    }
    else {
      activeModelIndex = index;
      activeModel = models[activeModelIndex];
    }
  }
}