#if __DIGGER__
using UnityEngine;
using Digger;

public class TerrainModifier : MonoBehaviour {
  // TODO: automate setup in script
  [TextArea] public string setupGuide = "To use this component, add Digger Master (Tools -> Digger -> Setup terrains) & Digger Master Runtime (Setup for runtime)";
  [SerializeField] private Camera currentCamera;

  [Header("Async parameters")]
  [Tooltip("Enable to edit the terrain asynchronously and avoid impacting the frame rate too much.")]
  public bool editAsynchronously = true;

  [Header("Modification parameters")]
  public BrushType brush = BrushType.Sphere;
  public ActionType action = ActionType.Dig;
  // TODO: get textures from DiggerMaster
  [Range(0, 7)] public int textureIndex;
  [Range(0.5f, 10f)] public float size = 4f;
  [Range(0f, 1f)] public float opacity = 0.5f;
  public bool autoRemoveTrees = true;
  public bool autoRemoveDetails = true;

  [Header("Persistence parameters (make sure persistence is enabled in Digger Master Runtime)")]
  public KeyCode persistDataKey = KeyCode.P;

  public KeyCode deleteDataKey = KeyCode.K;

  [Header("Editor-only parameters")]
  [Tooltip("Enable to remove trees while you dig in Play Mode in the Unity editor. " +
           "CAUTION: removal is permanent and will remain after you exit Play Mode!")]
  public bool autoRemoveTreesInEditor = false;

  [Tooltip("Enable to remove grass and details while you dig in Play Mode in the Unity editor. " +
           "CAUTION: removal is permanent and will remain after you exit Play Mode!")]
  public bool autoRemoveDetailsInEditor = false;

  private DiggerMasterRuntime diggerMasterRuntime;

  private void Start() {
    currentCamera = Camera.main;
    diggerMasterRuntime = FindObjectOfType<DiggerMasterRuntime>();
    if (!diggerMasterRuntime) {
      enabled = false;
      Debug.LogWarning(
          "DiggerRuntimeUsageExample component requires DiggerMasterRuntime component to be setup in the scene. DiggerRuntimeUsageExample will be disabled.");
      return;
    }
  }

  private void Update() {
    if (MouseButton.Left.IsHeld()) ProcessModifying();
    if (persistDataKey.IsUp()) PersisModifyingData();
    if (deleteDataKey.IsUp()) DeleteModifyingData();
  }

  public void ProcessModifying() {
    // Perform a raycast to find terrain surface
    if (Physics.Raycast(currentCamera.transform.position, currentCamera.transform.forward, out var hit, 2000f)) {
      if (editAsynchronously) ModifyAsync(hit);
      else Modify(hit);
    }
  }

  private void ModifyAsync(RaycastHit raycastHit) {
    diggerMasterRuntime.ModifyAsyncBuffured(raycastHit.point, brush, action, textureIndex, opacity, size,
                                            Application.isEditor ? autoRemoveDetailsInEditor : autoRemoveDetails,
                                            Application.isEditor ? autoRemoveTreesInEditor : autoRemoveTrees);
  }

  private void Modify(RaycastHit raycastHit) {
    diggerMasterRuntime.Modify(raycastHit.point, brush, action, textureIndex, opacity, size,
                               Application.isEditor ? autoRemoveDetailsInEditor : autoRemoveDetails,
                               Application.isEditor ? autoRemoveTreesInEditor : autoRemoveTrees);
  }

  private void PersisModifyingData() {
    diggerMasterRuntime.PersistAll();
#if !UNITY_EDITOR
      Debug.Log("Persisted all modified chunks");
#endif
  }

  private void DeleteModifyingData() {
    diggerMasterRuntime.DeleteAllPersistedData();
#if !UNITY_EDITOR
      Debug.Log("Deleted all persisted data");
#endif
  }
}
#endif
