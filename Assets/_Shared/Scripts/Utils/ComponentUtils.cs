using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public static class ComponentUtils {
  public static T CopyTo<T>(this T original, GameObject destination) where T : Component {
    System.Type type = original.GetType();
    Component copy = destination.AddComponent(type);
    System.Reflection.FieldInfo[] fields = type.GetFields();
    foreach (System.Reflection.FieldInfo field in fields) {
      field.SetValue(copy, field.GetValue(original));
    }
    return copy as T;
  }


  /// <summary> Return a linked copy of the component. When the copy changes, the component will update and vice versa</summary>
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
    // TODO: constrain
    return theRb;
  }

  /// <summary>If RigidBody exists, setup/override it as provided params</summary>
  public static Rigidbody AddAndSetupRigidBodyIfNotExist(this MonoBehaviour monoBehaviour, bool useGravity = true, float mass = 1, bool isKinematic = false) {
    Rigidbody theRb = monoBehaviour.gameObject.GetComponent<Rigidbody>();
    if (theRb == null) theRb = monoBehaviour.gameObject.AddComponent<Rigidbody>();
    theRb.useGravity = useGravity;
    theRb.mass = mass;
    theRb.isKinematic = isKinematic;
    // TODO: constrain
    return theRb;
  }

  /// <summary>
  /// Temporary disable the component for a given period.
  /// </summary>
  public static void Disable(this MonoBehaviour monoBehaviour, float timePeriod) {
    monoBehaviour.enabled = false;
    monoBehaviour.StartCoroutine(Enable(monoBehaviour, delay: timePeriod));
  }

  private static IEnumerator Enable(MonoBehaviour monoBehaviour, float delay) {
    yield return new WaitForSeconds(delay);
    monoBehaviour.enabled = true;
  }
}