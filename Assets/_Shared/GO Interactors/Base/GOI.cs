using System;
using System.Collections.Generic;
using System.Linq;
using Enginooby.Attribute;
using Enginooby.Utils;
using UnityEngine;

// REFACTOR: Extend Interactor<GameObject>
// TODO
// + Randomize effect

/// <summary>
///   GameObject Interactor base class.
/// </summary>
public abstract partial class GOI {
  protected List<GameObject> _interactedGos = new();

  public virtual List<GameObject> InteractedGos => _interactedGos;

  // TIP: Clear data of the singleton, otherwise it will remain through Play sessions.
  private void OnApplicationQuit() {
    // ! Instance of the derived Singleton is not null
    // print("ClearInteractedGos from " + gameObject.name);
    ClearInteractedGos();
  }

  protected virtual void ClearInteractedGos() => InteractedGos.Clear();

  public abstract void IncrementEffect();
  public abstract void DecrementEffect();

  // ? Use enum for different interact modes: Apply, Disable, Enable, Toggle (drawback: hard to use with UnityEvent)
  //* BASE
  /// <summary>
  ///   Apply the effect of Interactor on the given GameObject.<br />
  ///   E.g., select, highlight, ect.
  /// </summary>
  public abstract void Interact(GameObject go);

  /// <summary>
  ///   Remove/disable the effect of Interactor on the given GameObject.<br />
  ///   E.g., deselect, unhighlight, ect.
  /// </summary>
  public abstract void InteractRevert(GameObject go);

  /// <summary>
  ///   Re-enable the old effects if GO was interacted then reverted
  ///   (normally implementation is opposite to InteractRevert method).
  /// </summary>
  public abstract void InteractRestore(GameObject go);

  /// <summary>
  ///   Toggle the effect of Interactor on the given GameObject.
  /// </summary>
  public abstract void InteractToggle(GameObject go);

  //* MULTIPLE GAMEOBJECTS
  public virtual void Interact(IEnumerable<GameObject> gos) => gos.ForEach(Interact);
  public virtual void InteractRevert(IEnumerable<GameObject> gos) => gos.ForEach(InteractRevert);
  public virtual void InteractRestore(IEnumerable<GameObject> gos) => gos.ForEach(InteractRestore);
  public virtual void InteractToggle(IEnumerable<GameObject> gos) => gos.ForEach(InteractToggle);

  //* CACHED GAMEOBJECTS
  public virtual void InteractForInteracted() => Interact(InteractedGos);
  public virtual void InteractRevertForInteracted() => InteractRevert(InteractedGos);
  public virtual void InteractRestoreForInteracted() => InteractRestore(InteractedGos);
  public virtual void InteractToggleForInteracted() => InteractToggle(InteractedGos);

  //* GAMEOBJECT FILTER
  // TODO: Implement random, limit for each filter method
  public void InteractByName(string name) { }
  public void InteractByTag(string tag) { }
  public void InteractByType(Type type) { }
  public void InteractByTypeName(string typeName) { }
  public void InteractInsideArea(IArea area, GameObject origin) { }
  public void InteractOutsideArea(IArea area, GameObject origin) { }
}

/// <inheritdoc />
public abstract partial class GOI<TSelf> {
  // TIP: Get instance of derived Singleton using "Self-passing generics"
  // This replaces Instance in base
  public new static TSelf Instance => MonoBehaviourSingleton<TSelf>.Instance;

  // TIP: Validate type when using generic singleton to keep singletons of different generic types alive
  // E.g., if not do this, GOBlender, GOHighlighter, GOVFXer,... will be destroyed except one. 
  protected override bool DoesInstanceExist => _instance && _instance.GetType() == typeof(TSelf);

  [Button]
  public void InteractWithInteractables() {
    // TIP: "Self-passing generics" to implement generic method accepting "this" type
    var interactables = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable<TSelf>>();
    foreach (var interactable in interactables) // Interact(interactable.GameObject);
      interactable.OnInteracted();
  }
}