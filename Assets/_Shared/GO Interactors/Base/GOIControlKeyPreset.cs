using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
using Enginooby.Core;
#endif

// TODO
// + Implement global key setup
// + Auto update asset name

[CreateAssetMenu(fileName = "InteractControlKeyPreset", menuName = "InteractControlKeyPreset", order = 0)]
/// <summary>
/// Preset of input keys for InteractController for easy swapping.
/// </summary>
public class GOIControlKeyPreset : SerializedScriptableObject {
  [BoxGroup("Effect")] [SerializeField] private InputModifier _incrementEffectKey = new();

  [BoxGroup("Effect")] [SerializeField] private InputModifier _decrementEffectKey = new();


  [BoxGroup("Raycast")] [SerializeField]
  private bool _enablePassThrough = true; // if disable, don't interact GOs behind the 1st one.

  [BoxGroup("Raycast")] [SerializeField] private InputModifier _interactKey = new();

  [BoxGroup("Raycast")] [SerializeField] private InputModifier _revertKey = new();

  [BoxGroup("Raycast")] [SerializeField] private InputModifier _toggleKey = new();


  [BoxGroup("Selected")] [SerializeField]
  private InputModifier _interactSelectedKey = new();

  [BoxGroup("Selected")] [SerializeField]
  private InputModifier _toggleSelectedKey = new();

  [BoxGroup("Selected")] [SerializeField]
  private InputModifier _revertSelectedKey = new();


  [BoxGroup("Interacted")] [SerializeField]
  private InputModifier _interactInteractedKey = new();

  [BoxGroup("Interacted")] [SerializeField]
  private InputModifier _toggleInteractedKey = new();

  [BoxGroup("Interacted")] [SerializeField]
  private InputModifier _revertInteractedKey = new();

  [BoxGroup("Interacted")] [SerializeField]
  private InputModifier _restoreInteractedKey = new();

  public InputModifier IncrementEffectKey => _incrementEffectKey;
  public InputModifier DecrementEffectKey => _decrementEffectKey;

  public bool EnablePassThrough => _enablePassThrough;
  public InputModifier InteractKey => _interactKey;
  public InputModifier RevertKey => _revertKey;
  public InputModifier ToggleKey => _toggleKey;

  public InputModifier InteractSelectedKey => _interactSelectedKey;
  public InputModifier ToggleSelectedKey => _toggleSelectedKey;
  public InputModifier RevertSelectedKey => _revertSelectedKey;

  public InputModifier InteractInteractedKey => _interactInteractedKey;
  public InputModifier ToggleInteractedKey => _toggleInteractedKey;
  public InputModifier RevertInteractedKey => _revertInteractedKey;
  public InputModifier RestoreInteractedKey => _restoreInteractedKey;
}