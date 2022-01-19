using Sirenix.OdinInspector;
using UnityEngine;

// TODO
// + Implement global key setup
// + Auto update asset name

[CreateAssetMenu(fileName = "InteractControlKeyPreset", menuName = "InteractControlKeyPreset", order = 0)]
/// <summary>
/// Preset of input keys for InteractController for easy swapping.
/// </summary>
public class GOIControlKeyPreset : SerializedScriptableObject {
  [BoxGroup("Effect")]
  [SerializeField]
  private InputModifier _incrementEffectKey = new InputModifier();
  [BoxGroup("Effect")]
  [SerializeField]
  private InputModifier _decrementEffectKey = new InputModifier();

  public InputModifier IncrementEffectKey => _incrementEffectKey;
  public InputModifier DecrementEffectKey => _decrementEffectKey;


  [BoxGroup("Raycast")]
  [SerializeField]
  private bool _enablePassThrough = true; // if disable, don't interact GOs behind the 1st one.
  [BoxGroup("Raycast")]
  [SerializeField]
  private InputModifier _interactKey = new InputModifier();
  [BoxGroup("Raycast")]
  [SerializeField]
  private InputModifier _revertKey = new InputModifier();
  [BoxGroup("Raycast")]
  [SerializeField]
  private InputModifier _toggleKey = new InputModifier();

  public bool EnablePassThrough => _enablePassThrough;
  public InputModifier InteractKey => _interactKey;
  public InputModifier RevertKey => _revertKey;
  public InputModifier ToggleKey => _toggleKey;


  [BoxGroup("Selected")]
  [SerializeField]
  private InputModifier _interactSelectedKey = new InputModifier();
  [BoxGroup("Selected")]
  [SerializeField]
  private InputModifier _toggleSelectedKey = new InputModifier();
  [BoxGroup("Selected")]
  [SerializeField]
  private InputModifier _revertSelectedKey = new InputModifier();

  public InputModifier InteractSelectedKey => _interactSelectedKey;
  public InputModifier ToggleSelectedKey => _toggleSelectedKey;
  public InputModifier RevertSelectedKey => _revertSelectedKey;


  [BoxGroup("Interacted")]
  [SerializeField]
  private InputModifier _interactInteractedKey = new InputModifier();
  [BoxGroup("Interacted")]
  [SerializeField]
  private InputModifier _toggleInteractedKey = new InputModifier();
  [BoxGroup("Interacted")]
  [SerializeField]
  private InputModifier _revertInteractedKey = new InputModifier();
  [BoxGroup("Interacted")]
  [SerializeField]
  private InputModifier _restoreInteractedKey = new InputModifier();

  public InputModifier InteractInteractedKey => _interactInteractedKey;
  public InputModifier ToggleInteractedKey => _toggleInteractedKey;
  public InputModifier RevertInteractedKey => _revertInteractedKey;
  public InputModifier RestoreInteractedKey => _restoreInteractedKey;
}