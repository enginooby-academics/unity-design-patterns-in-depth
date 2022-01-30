using UnityEngine;

public class SelectionController : MonoBehaviour {
  [SerializeField] private KeyCodeModifier deleteKey = new KeyCodeModifier(keyCode: KeyCode.Delete);
  [SerializeField] private KeyCodeModifier movePlayerToKey = new KeyCodeModifier(keyCode: KeyCode.F);
  [SerializeField] private KeyCodeModifier moveToPlayerKey = new KeyCodeModifier(keyCode: KeyCode.V);
  [SerializeField] private KeyCodeModifier revertPlayerPosKey = new KeyCodeModifier(keyCode: KeyCode.G);
  [SerializeField] private KeyCodeModifier toggleVfxKey = new KeyCodeModifier(keyCode: KeyCode.T);
  [SerializeField] private KeyCodeModifier enablePreviousVfxKey = new KeyCodeModifier(keyCode: KeyCode.R);
  [SerializeField] private KeyCodeModifier enableNextVfxKey = new KeyCodeModifier(keyCode: KeyCode.Y);

#if ASSET_MESH_EFFECTS
  [Header("[Directors]")]
  [SerializeField] private Selector selector;
  [SerializeField] private Highlighter highlighter;
  [SerializeField] private Transformer transformer;
  [SerializeField] private Vfxer vfxer;

  void Start() {
    // CONSIDER: find references in Edit Mode
    selector ??= FindObjectOfType<Selector>();
    highlighter ??= FindObjectOfType<Highlighter>();
    transformer ??= FindObjectOfType<Transformer>();
    vfxer ??= FindObjectOfType<Vfxer>();
  }

  void Update() {
    if (revertPlayerPosKey.IsTriggering) transformer?.RevertPlayerPosition();

    if (!selector || !selector.CurrentSelectedObject) return;

    GameObject selection = selector.CurrentSelectedObject;
    // TODO: Change cursor to recycle bin
    if (deleteKey.IsTriggering) Destroy(selection);
    if (movePlayerToKey.IsTriggering) transformer?.MovePlayerTowards(selection);
    if (moveToPlayerKey.IsTriggering) transformer?.MoveTowardsPlayer(selection);
    if (toggleVfxKey.IsTriggering) vfxer?.ToggleActiveMeshEffect(selection);
    if (enablePreviousVfxKey.IsTriggering) vfxer?.AddPreviousMeshEffect(selection);
    if (enableNextVfxKey.IsTriggering) vfxer?.AddNextMeshEffect(selection);
  }

  [QFSW.QC.Command(CommandPrefix.Selection + "mvfx")]
  [QFSW.QC.Command(CommandPrefix.MeshVfx + CommandTarget.Selection)]
  public void AddMeshEffectToSelection(int effectId) {
    vfxer?.AddMeshEffect(selector.CurrentSelectedObject, effectId);
  }

  [QFSW.QC.Command(CommandPrefix.Selection + "mvfx-fire")]
  [QFSW.QC.Command(CommandPrefix.MeshVfx + CommandTarget.Selection + "-fire")]
  public void AddFireMeshEffectToSelection(int effectId = -1) {
    if (effectId == -1) vfxer?.AddFireMeshEffect(selector.CurrentSelectedObject);
    else vfxer?.AddFireMeshEffect(selector.CurrentSelectedObject, effectId);
  }
#endif
}
