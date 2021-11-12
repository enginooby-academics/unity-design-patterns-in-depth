// * Manage all events of each GameObject
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// ! Cannot bind events in Inspector if the listener is Prefab Asset. Workaround: only use Scene Prefab
public class EventManager : MonoBehaviour {
  public List<OnTriggerEnterEvent> onTriggerEnterEvents = new List<OnTriggerEnterEvent>();
  public List<OnCollisionEnterEvent> onCollisionEnterEvents = new List<OnCollisionEnterEvent>();
  public List<OnMouseDownEvent> onMouseDownEvents = new List<OnMouseDownEvent>();


  private IEnumerator OnTriggerEnter(Collider other) {
    for (int i = 0; i < onTriggerEnterEvents.Count; i++) {
      OnTriggerEnterEvent e = onTriggerEnterEvents[i];
      // TODO: Refactor
      if (!e.enable || !e.IsTriggerBy(other.gameObject)) continue;
      PreprocessEventUpdateNameAndTag(e, other.gameObject);
      yield return new WaitForSeconds(e.delay);
      e.Invoke();
      ProcessEventFX(e);
      ProcessAfterInvoke(e, other: other.gameObject);
    }
  }

  private IEnumerator OnCollisionEnter(Collision other) {
    for (int i = 0; i < onCollisionEnterEvents.Count; i++) {
      OnCollisionEnterEvent e = onCollisionEnterEvents[i];
      // TODO: Refactor
      if (!e.enable || !e.IsTriggerBy(other.gameObject)) continue;
      PreprocessEventUpdateNameAndTag(e, other.gameObject);
      yield return new WaitForSeconds(e.delay);
      e.Invoke();
      ProcessEventFX(e);
      ProcessAfterInvoke(e, other: other.gameObject);
    }
  }

  private IEnumerator OnMouseDown() {
    // ! Events do not invoke async
    for (int i = 0; i < onMouseDownEvents.Count; i++) {
      OnMouseDownEvent e = onMouseDownEvents[i];
      if (!e.enable) continue;
      yield return new WaitForSeconds(e.delay);
      e.Invoke();
      ProcessEventFX(e);
      ProcessAfterInvoke(e);
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
      ParticleSystem vfx = Instantiate(triggerEvent.Vfx, transform.position, triggerEvent.Vfx.transform.rotation);
      Destroy(vfx.gameObject, 5f);
    }
    if (triggerEvent.Sfx) triggerEvent.Sfx.PlayOneShot();
  }

  private void ProcessAfterInvoke(TriggerEvent triggerEvent, GameObject other = null) {
    if (triggerEvent.destroyAfterInvoked.HasFlag(DestroyTarget.Trigger) && this) Destroy(this);
    if (triggerEvent.destroyAfterInvoked.HasFlag(DestroyTarget.Self) && gameObject) Destroy(gameObject);
    if (triggerEvent.destroyAfterInvoked.HasFlag(DestroyTarget.Other) && other) Destroy(other);

    if (triggerEvent.releaseToPoolTargetAfterInvoked.HasFlag(ReleaseToPoolTarget.Self)) {
      PoolObject poolObject = GetComponent<PoolObject>(); ;
      if (!poolObject) return;
      poolObject.ReleaseToPool();
    }

    if (triggerEvent.releaseToPoolTargetAfterInvoked.HasFlag(ReleaseToPoolTarget.Other)) {
      PoolObject poolObject = other.GetComponent<PoolObject>(); ;
      if (!poolObject) return;
      poolObject.ReleaseToPool();
    }
  }
}
