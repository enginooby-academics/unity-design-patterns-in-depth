using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public static class ComponentUtils {
  /// <summary>
  /// Destroy this component's GameObject if component is not null (safely).
  /// </summary>
  public static void DestroyGameObject(this Component component) {
    if (component != null) UnityEngine.Object.Destroy(component.gameObject);
  }

  /// <summary>
  /// Destroy all GameObjects of components if component is not null.
  /// </summary>
  public static void DestroyGameObjects(this Component[] components) {
    foreach (var component in components) {
      component.DestroyGameObject();
    }
  }

  /// <summary>
  /// Get all components whose GameObject is child of this GameObject (not this GameObject).
  /// </summary>
  public static List<T> GetComponentsInChildrenOnly<T>(this GameObject gameObject) where T : Component {
    List<T> components = new List<T>();
    foreach (T component in gameObject.transform) {
      components.Add(component);
    }

    return components;
  }

  /// <summary>
  /// Get all child GameObjects of this GameObject (not this GameObject).
  /// </summary>
  public static List<GameObject> GetGameObjectsInChildrenOnly(this GameObject gameObject) {
    List<GameObject> children = new List<GameObject>();
    foreach (Transform transformChild in gameObject.transform) {
      children.Add(transformChild.gameObject);
    }

    return children;
  }

  public static T CopyTo<T>(this T original, GameObject destination) where T : Component {
    System.Type type = original.GetType();
    Component copy = destination.AddComponent(type);
    System.Reflection.FieldInfo[] fields = type.GetFields();
    foreach (System.Reflection.FieldInfo field in fields) {
      field.SetValue(copy, field.GetValue(original));
    }
    return copy as T;
  }


  /// <summary> 
  /// [Reflection] <br/>
  /// Return a linked copy of the component. When the copy changes, the component will update and vice versa.
  /// </summary>
  public static T GetLinkedCopyOf<T>(this Component comp, T other) where T : Component {
    Type type = comp.GetType();
    if (type != other.GetType()) return null; // type mis-match
    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
    PropertyInfo[] pinfos = type.GetProperties(flags);
    foreach (var pinfo in pinfos) {
      if (pinfo.CanWrite) {
        try {
          pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
        } catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
      }
    }
    FieldInfo[] finfos = type.GetFields(flags);
    foreach (var finfo in finfos) {
      finfo.SetValue(comp, finfo.GetValue(other));
    }
    return comp as T;
  }

  /// <summary>If RigidBody exists, just leave it as it be (respect the exsiting RigidBody)</summary>
  public static Rigidbody AddRigidBodyIfNotExist(this MonoBehaviour monoBehaviour, bool useGravity = true, float mass = 1) {
    Rigidbody theRb = monoBehaviour.gameObject.GetComponent<Rigidbody>();
    if (theRb != null) return theRb;
    theRb = monoBehaviour.gameObject.AddComponent<Rigidbody>();
    theRb.useGravity = useGravity;
    theRb.mass = mass;
    // TODO: constraint
    return theRb;
  }

  /// <summary>If RigidBody exists, setup & override it as provided params.</summary>
  public static Rigidbody AddAndSetupRigidBodyIfNotExist(this MonoBehaviour monoBehaviour, bool useGravity = true, float mass = 1, bool isKinematic = false) {
    Rigidbody theRb = monoBehaviour.gameObject.GetComponent<Rigidbody>();
    if (theRb == null) theRb = monoBehaviour.gameObject.AddComponent<Rigidbody>();
    theRb.useGravity = useGravity;
    theRb.mass = mass;
    theRb.isKinematic = isKinematic;
    // TODO: constraint
    return theRb;
  }

  /// <summary>
  /// If Collider exists, setup & override it as provided params. Otherwise, add BoxCollider.
  /// </summary>
  public static Collider AddAndSetupColliderIfNotExist(this MonoBehaviour monoBehaviour, bool isTrigger = true) {
    return monoBehaviour.gameObject.AddAndSetupColliderIfNotExist(isTrigger);
  }

  /// <summary>
  /// If Collider exists, setup & override it as provided params. Otherwise, add BoxCollider.
  /// </summary>
  public static Collider AddAndSetupColliderIfNotExist(this GameObject gameObject, bool isTrigger = true) {
    return gameObject.AddAndSetupColliderIfNotExist<BoxCollider>(isTrigger);
  }

  /// <summary>
  /// If Collider exists, setup & override it as provided params.
  /// </summary>
  public static Collider AddAndSetupColliderIfNotExist<T>(this GameObject gameObject, bool isTrigger = true) where T : Collider {
    Collider collider = gameObject.GetComponent<Collider>();
    if (collider == null) collider = gameObject.AddComponent<T>();
    collider.isTrigger = isTrigger;
    return collider;
  }

  /// <summary>
  /// Temporarily disable the component for a given period.
  /// </summary>
  public static void Disable(this MonoBehaviour monoBehaviour, float timePeriod) {
    monoBehaviour.enabled = false;
    monoBehaviour.StartCoroutine(EnableCoroutine(monoBehaviour, delay: timePeriod));
  }

  private static IEnumerator EnableCoroutine(MonoBehaviour monoBehaviour, float delay) {
    yield return new WaitForSeconds(delay);
    monoBehaviour.enabled = true;
  }
}