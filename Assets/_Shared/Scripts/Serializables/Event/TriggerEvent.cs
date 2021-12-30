using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

[Flags] public enum DestroyTarget { Trigger = 1 << 1, Self = 1 << 2, Other = 1 << 3 }
[Flags] public enum ReleaseToPoolTarget { Self = 1 << 1, Other = 1 << 2 }

[Serializable, InlineProperty]
public class TriggerEvent {
  [HorizontalGroup("Group 1")]
  [PropertyOrder(-10)]
  public bool enable = true;

  [HorizontalGroup("Group 1")]
  [EnableIf(nameof(enable))]
  [PropertyOrder(-10)]
  public float delay;

  // ? Make tag list as Serialiable or part of TargetTags
  // [ShowIf(nameof(eventType), EventType.OnCollisionEnter)]
  // [SerializeField, LabelText("Tags")] public List<string> onCollisionEnterTags = new List<string>() { "Player" };

  [EnableIf(nameof(enable))]
  public UnityEvent Action = new UnityEvent();

  [EnableIf(nameof(enable))]
  [Tooltip("If Destroy Self After Invoke, does not set VFX as child since it'll be destroyed as well.")]
  [LabelText("VFX")] public ParticleSystem Vfx;
  [EnableIf(nameof(enable))]
  [LabelText("SFX")] public AudioClip Sfx;

  [EnableIf(nameof(enable))]
  [BoxGroup("After Action")]
  [ValidateInput(nameof(ValidDestroyTarget), "Destroy target is not proper to the current event!", InfoMessageType.Warning)]
  [EnumToggleButtons, LabelText("Destroy")] public DestroyTarget destroyAfterInvoked;
  private bool ValidDestroyTarget() {
    // if (eventType == EventType.OnMouseDown && destroyAfterInvoked.HasFlag(DestroyTarget.Other)) {
    //   return false;
    // }
    return true;
  }

  [EnableIf(nameof(enable))]
  [BoxGroup("After Action")]
  [EnumToggleButtons, LabelText("Release To Pool")] public ReleaseToPoolTarget releaseToPoolTargetAfterInvoked;

  [EnableIf(nameof(enable))]
  [BoxGroup("After Action")]
  [Tooltip("Changing tag is useful to make the event trigger only once w/o distroying target/collider. Not set if leave field empty.")]
  [LabelText("Set Other Tag")] public string otherNewTag;

  [EnableIf(nameof(enable))]
  [BoxGroup("After Action")]
  [Tooltip("Not set if leave field empty.")]
  [LabelText("Set Other Name")] public string otherNewName;


  // TODO: Make Serialize Wrapper for ParticaleSystem (play pos, scale, set parent) & AudioClip (play pos, volume, repeat)




  /// <summary>
  /// Execute common code for all events.
  /// </summary>
  public void Invoke() {
    Action.Invoke();
  }
}