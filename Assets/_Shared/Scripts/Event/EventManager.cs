using System.Collections;
using System.Collections.Generic;
using Enginooby;
using UnityEngine;

// ! Cannot bind events in Inspector if the listener is Prefab Asset. Workaround: only use Scene Prefab
// ? Rename: GameObjectEventManager
/// <summary>
///   Manage all events for an individual GameObject.
/// </summary>
public class EventManager : MonoBehaviour {
  public List<OnTriggerEnterEvent> onTriggerEnterEvents = new();
  public List<OnCollisionEnterEvent> onCollisionEnterEvents = new();
  public List<OnMouseDownEvent> onMouseDownEvents = new();

  private IEnumerator OnCollisionEnter(Collision other) {
    foreach (var @event in onCollisionEnterEvents) {
      // TODO: Refactor
      if (!@event.enable || !@event.IsTriggerBy(other.gameObject)) continue;
      PreprocessEventUpdateNameAndTag(@event, other.gameObject);
      yield return new WaitForSeconds(@event.delay);
      @event.Invoke();
      ProcessEventFX(@event);
      ProcessAfterInvoke(@event, other.gameObject);
    }
  }

  private IEnumerator OnMouseDown() {
    // ! Events do not invoke async
    foreach (var e in onMouseDownEvents) {
      if (!e.enable) continue;
      yield return new WaitForSeconds(e.delay);
      e.Invoke();
      ProcessEventFX(e);
      ProcessAfterInvoke(e);
    }
  }


  private IEnumerator OnTriggerEnter(Collider other) {
    foreach (var @event in onTriggerEnterEvents) {
      // TODO: Refactor
      if (!@event.enable || !@event.IsTriggerBy(other.gameObject)) continue;
      PreprocessEventUpdateNameAndTag(@event, other.gameObject);
      yield return new WaitForSeconds(@event.delay);
      @event.Invoke();
      ProcessEventFX(@event);
      ProcessAfterInvoke(@event, other.gameObject);
    }
  }

  private void PreprocessEventUpdateNameAndTag(TriggerEvent triggerEvent, GameObject other) {
    // UTIL
    if (!string.IsNullOrWhiteSpace(triggerEvent.otherNewName)) other.name = triggerEvent.otherNewName;
    if (!string.IsNullOrWhiteSpace(triggerEvent.otherNewTag)) other.tag = triggerEvent.otherNewTag;
  }

  private void ProcessEventFX(TriggerEvent triggerEvent) {
    if (triggerEvent.Vfx) {
      // triggerEvent.Vfx.Play();
      var vfx = Instantiate(triggerEvent.Vfx, transform.position, triggerEvent.Vfx.transform.rotation);
      Destroy(vfx.gameObject, 5f);
    }

    if (triggerEvent.Sfx) triggerEvent.Sfx.PlayOneShot();
  }

  private void ProcessAfterInvoke(TriggerEvent triggerEvent, GameObject other = null) {
    if (triggerEvent.destroyAfterInvoked.HasFlag(DestroyTarget.Trigger) && this) Destroy(this);
    if (triggerEvent.destroyAfterInvoked.HasFlag(DestroyTarget.Self) && gameObject) Destroy(gameObject);
    if (triggerEvent.destroyAfterInvoked.HasFlag(DestroyTarget.Other) && other) Destroy(other);

    if (triggerEvent.releaseToPoolTargetAfterInvoked.HasFlag(ReleaseToPoolTarget.Self)) {
      var poolObject = GetComponent<PoolObject>();
      if (!poolObject) return;
      poolObject.ReleaseToPool();
    }

    if (triggerEvent.releaseToPoolTargetAfterInvoked.HasFlag(ReleaseToPoolTarget.Other) && other) {
      var poolObject = other.GetComponent<PoolObject>();
      if (!poolObject) return;
      poolObject.ReleaseToPool();
    }
  }
}