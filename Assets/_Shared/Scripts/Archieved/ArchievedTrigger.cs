// ! Replace by TriggerManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;

// TODO
// + SFX/VFX according to event
// + Time delay for destroyAfterInvoked & destroyOtherAfterInvoked
// + Condition for destroyOtherAfterInvoked (OnTrigger, OnCollision)
// + Separate delay for each TriggerEvent
// + Helper editor to setup collider/rigidbody (based on axis , shape & size), remove MeshRenderer (or other components for area trigger type)
// + Helper function to add tags (player, bullet...)

// CONSIDER: define TriggerEvent class to separate DestroyAfterInvoked, delay for each type
// class TriggerEvent {
//   TriggerEventType type;
//   float delay;
//   DestroyTarget destroyAfterInvoked;
// }


public abstract class ArchievedTrigger : MonoBehaviour {

  [InfoBox("Destroy after invoked to avoid trigger multiple times at one place causing undesire effect (e.g. Game Over trigger). Cautious: destroy self will also destroy VFX attach with it.")]
  [SerializeField, EnumToggleButtons] protected DestroyTarget destroyAfterInvoked;

  [Space(10)]
  [EnumToggleButtons]
  // [LabelText("Event")]
  [SerializeField] protected TriggerEventType triggerEvent = TriggerEventType.OnCollisionEnter;

  #region ON COLLISION ENTER
  [ShowIfGroup(nameof(IsTriggerEventOnCollisionEnter))]
  [BoxGroup(nameof(IsTriggerEventOnCollisionEnter) + "/OnCollisionEnter")]
  [SerializeField, LabelText("Delay"), Min(0f)] protected float onCollisionEnterDelay;

  [BoxGroup(nameof(IsTriggerEventOnCollisionEnter) + "/OnCollisionEnter")]
  [SerializeField] protected List<string> onCollisionEnterTriggerTags = new List<string>() { "Player" };

  [BoxGroup(nameof(IsTriggerEventOnCollisionEnter) + "/OnCollisionEnter")]
  [SerializeField] UnityEvent onCollisionEnterManualEvent;

  // TIP: use Property (get) for quick checking class attribute, rather than use function
  private bool IsTriggerEventOnCollisionEnter { get => triggerEvent.HasFlag(TriggerEventType.OnCollisionEnter); }

  private IEnumerator OnCollisionEnter(Collision other) {
    if (!IsTriggerEventOnCollisionEnter) yield break;

    if (onCollisionEnterTriggerTags.Contains(other.gameObject.tag)) {
      yield return new WaitForSeconds(onCollisionEnterDelay);
      InvokeAction();
      if (destroyAfterInvoked.HasFlag(DestroyTarget.Other)) Destroy(other.gameObject);
    }
    onCollisionEnterManualEvent.Invoke(); // QUESTION: is it delayed for this as well?
  }
  #endregion


  #region ON COLLISION EXIT
  [TabGroup("OnCollisionExit")]
  [ShowIf(nameof(IsTriggerEventOnCollisionExit))]
  [SerializeField] protected List<string> onCollisionExitTriggerTags = new List<string>() { "Player" };

  [TabGroup("OnCollisionExit")]
  [ShowIf(nameof(IsTriggerEventOnCollisionExit))]
  [SerializeField] UnityEvent onCollisionExitManualEvent;

  private bool IsTriggerEventOnCollisionExit { get => triggerEvent.HasFlag(TriggerEventType.OnCollisionExit); }
  #endregion

  #region ON TRIGGER ENTER
  [ShowIfGroup(nameof(IsTriggerEventOnTriggerEnter))]
  [BoxGroup(nameof(IsTriggerEventOnTriggerEnter) + "/OnTriggerEnter")]
  [SerializeField, LabelText("Delay"), Min(0f)] protected float onTriggerEnterDelay;
  [BoxGroup(nameof(IsTriggerEventOnTriggerEnter) + "/OnTriggerEnter")]
  [SerializeField] [LabelText("VFX")] ParticleSystem onTriggerEnterVfx;
  [BoxGroup(nameof(IsTriggerEventOnTriggerEnter) + "/OnTriggerEnter")]
  [SerializeField] [LabelText("SFX")] AudioClip onTriggerEnterSfx;
  [BoxGroup(nameof(IsTriggerEventOnTriggerEnter) + "/OnTriggerEnter")]
  [SerializeField] protected List<string> onTriggerEnterTriggerTags = new List<string>() { "Player" };

  [BoxGroup(nameof(IsTriggerEventOnTriggerEnter) + "/OnTriggerEnter")]
  [SerializeField] UnityEvent onTriggerEnterManualEvent;

  private bool IsTriggerEventOnTriggerEnter { get => triggerEvent.HasFlag(TriggerEventType.OnTriggerEnter); }

  private IEnumerator OnTriggerEnter(Collider other) {
    if (!IsTriggerEventOnTriggerEnter) yield break;
    if (onCollisionEnterTriggerTags.Contains(other.tag)) {
      yield return new WaitForSeconds(onTriggerEnterDelay);
      InvokeAction();
      PlayTriggerEventVfx(onTriggerEnterVfx);
      if (onTriggerEnterSfx) GameManager.Instance.audioSource.PlayOneShot(onTriggerEnterSfx);
      if (destroyAfterInvoked.HasFlag(DestroyTarget.Other)) Destroy(other.gameObject);
    }

    onTriggerEnterManualEvent.Invoke();
  }

  private void PlayTriggerEventVfx(ParticleSystem? vfx) {
    if (!vfx) return;
    if (destroyAfterInvoked.HasFlag(DestroyTarget.Self)) {
      if (vfx.gameObject.transform.parent = gameObject.transform) {
        vfx.gameObject.transform.parent = gameObject.transform.parent;
        Destroy(vfx.gameObject, 3f);
      }
    }
    vfx.Play();
  }
  #endregion


  #region ON TRIGGER EXIT
  [ShowIf(nameof(IsTriggerEventOnTriggerExit))]
  [SerializeField] protected List<string> onTriggerExitTriggerTags = new List<string>() { "Player" };
  private bool IsTriggerEventOnTriggerExit { get => triggerEvent.HasFlag(TriggerEventType.OnTriggerExit); }

  #endregion


  #region ON MOUSE ENTER
  #endregion


  #region ON MOUSE EXIT
  #endregion


  #region ON DESTROY
  private bool IsTriggerEventOnDestroy { get => triggerEvent.HasFlag(TriggerEventType.OnDestroy); }

  private void OnDestroy() {
    if (!IsTriggerEventOnDestroy) return;
    InvokeAction();
  }

  #endregion


  #region ON DISABLE
  #endregion

  /// <summary> Main action of the trigger </summary>
  public void InvokeAction() {
    InvokeActionImpl();
    // common procedure after invoking
    if (destroyAfterInvoked.HasFlag(DestroyTarget.Trigger)) Destroy(this);
    if (destroyAfterInvoked.HasFlag(DestroyTarget.Self)) Destroy(gameObject);
  }

  /// <summary> Specific case of the main trigger action in derived classes </summary>
  public abstract void InvokeActionImpl();

  #region SETUP HELPERS - common target for interactions
  [Button]
  [BoxGroup("Setup Helpers")]
  [GUIColor(.6f, .6f, 1f)]
  private void OnTouchBullet() {
    triggerEvent = TriggerEventType.OnCollisionEnter;
    onCollisionEnterTriggerTags = new List<string>() { "Bullet" };
  }

  [Button]
  [BoxGroup("Setup Helpers")]
  [GUIColor(.6f, .6f, 1f)]
  private void OnTouchPlayer() {
    triggerEvent = TriggerEventType.OnCollisionEnter;
    onCollisionEnterTriggerTags = new List<string>() { "Player" };
  }
  [Button]
  [BoxGroup("Setup Helpers")]
  [GUIColor(.6f, .6f, 1f)]
  private void OnTouchEnemy() {
    triggerEvent = TriggerEventType.OnCollisionEnter;
    onCollisionEnterTriggerTags = new List<string>() { "Enemy" };
  }
  [InfoBox("If use trigger in child of parent which has other type of trigger, then set RigidBody as Kinematic for child to avoid trigger in child also trigger in parent (Compound Colliders)")]
  // ref: https://answers.unity.com/questions/410711/trigger-in-child-object-calls-ontriggerenter-in-pa.html
  [Button]
  [BoxGroup("Setup Helpers")]
  [GUIColor(.6f, 1f, .6f)]
  private void SetupKinematic() {
    this.AddAndSetupRigidBodyIfNotExist(isKinematic: true);
  }
  #endregion
}
