using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ComponentUtils {
  /// <summary>
  ///   Destroy all GameObjects of the given <paramref name="components"/>.
  /// </summary>
  public static void DestroyWithGameObject<T>(this IEnumerable<T> components) where T : Component {
    foreach (var component in components) {
#if UNITY_EDITOR
      if (!EditorApplication.isPlaying) Object.DestroyImmediate(component.gameObject);
#endif
      Object.Destroy(component.gameObject);
    }
  }

  /// <inheritdoc cref="DestroyWithGameObject{T}"/>
  public static void DestroyGameObjects(this IEnumerable<Component> components) {
    foreach (var component in components) component.DestroyGameObject();
  }

  // ? Naming convention: "action performed on an object along with object's dependency"
  // ? Rename: DestroyWithGameObject
  /// <summary>
  ///   Destroy this component's GameObject if it isn't null (safely).
  /// </summary>
  public static void DestroyGameObject(this Component component, float delayInSec = 0f) {
    if (component != null) Object.Destroy(component.gameObject, delayInSec);
  }

  // ? Should have extension method for short code
  /// <summary>
  ///   Get all components whose GameObject is child of this GameObject (not on this GameObject).
  /// </summary>
  public static IEnumerable<T> GetComponentsInChildrenOnly<T>(this GameObject go) where T : Component =>
    go.transform.Cast<T>();

  /// <summary>
  ///   Get all child GameObjects of this GameObject (excluding this GameObject).
  /// </summary>
  public static IEnumerable<GameObject> GetGameObjectsInChildrenOnly(this GameObject go) =>
    from Transform transformChild in go.transform select transformChild.gameObject;

  // TIP: Use tag (remarks) in XML docs for method needed attention on usage
  // E.g., Reflection: methods should be used rarely due to performance cost.
  // Safe-update: methods are safe to use in Update() w/o harming performance, mostly thanks to dirty flag.
  // In-update: methods are supposed to be used inside Update(), usually included deltaTime.
  // Out-update: methods are supposed to be used outside Update(), usually involving coroutine.

  /// <summary>
  ///  <remarks>Reflection method</remarks>
  /// </summary>
  public static T CopyTo<T>(this T original, GameObject destination) where T : Component {
    var type = original.GetType();
    var copy = destination.AddComponent(type);
    var fields = type.GetFields();
    foreach (var field in fields) field.SetValue(copy, field.GetValue(original));
    return copy as T;
  }

  /// <summary>
  ///  <remarks>Reflection method</remarks>
  ///   Return a linked copy of the component. When the copy changes, the component will update and vice versa.
  /// </summary>
  public static T GetLinkedCopyOf<T>(this Component component, T other) where T : Component {
    var type = component.GetType();
    if (type != other.GetType()) return null; // type mis-match

    const BindingFlags flags = BindingFlags.Public |
                               BindingFlags.NonPublic |
                               BindingFlags.Instance |
                               BindingFlags.Default |
                               BindingFlags.DeclaredOnly;
    var propertyInfos = type.GetProperties(flags);
    foreach (var propertyInfo in propertyInfos)
      if (propertyInfo.CanWrite)
        try {
          propertyInfo.SetValue(component, propertyInfo.GetValue(other, null), null);
        }
        catch (Exception) {
          // ignored
        } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.

    var fieldInfos = type.GetFields(flags);
    foreach (var fieldInfo in fieldInfos) fieldInfo.SetValue(component, fieldInfo.GetValue(other));
    return component as T;
  }

  // ? Naming convention / API design: "action to add component if the object doesn't has it then return component"
  /// <summary>
  ///   If RigidBody exists, just leave it as it be (respect the existing RigidBody)
  /// </summary>
  public static Rigidbody AddRigidBodyIfNotExist(
    this MonoBehaviour monoBehaviour,
    bool useGravity = true,
    float mass = 1) {
    var theRb = monoBehaviour.gameObject.GetComponent<Rigidbody>();
    if (theRb != null) return theRb;
    theRb = monoBehaviour.gameObject.AddComponent<Rigidbody>();
    theRb.useGravity = useGravity;
    theRb.mass = mass;
    // TODO: constraint
    return theRb;
  }

  /// <summary>
  ///   If RigidBody exists, setup & override it as provided params.
  /// </summary>
  public static Rigidbody AddAndSetupRigidBodyIfNotExist(
    this MonoBehaviour monoBehaviour,
    bool useGravity = true,
    float mass = 1,
    bool isKinematic = false) {
    var theRb = monoBehaviour.gameObject.GetComponent<Rigidbody>();
    if (theRb == null) theRb = monoBehaviour.gameObject.AddComponent<Rigidbody>();
    theRb.useGravity = useGravity;
    theRb.mass = mass;
    theRb.isKinematic = isKinematic;
    // TODO: constraint
    return theRb;
  }

  /// <summary>
  ///   If Collider exists, setup & override it as provided params. Otherwise, add BoxCollider.
  /// </summary>
  public static Collider AddAndSetupColliderIfNotExist(this MonoBehaviour monoBehaviour, bool isTrigger = true) =>
    monoBehaviour.gameObject.AddAndSetupColliderIfNotExist(isTrigger);

  /// <summary>
  ///   If Collider exists, setup & override it as provided params. Otherwise, add BoxCollider.
  /// </summary>
  public static Collider AddAndSetupColliderIfNotExist(this GameObject go, bool isTrigger = true) =>
    go.AddAndSetupColliderIfNotExist<BoxCollider>(isTrigger);

  // ? Too many responsibilities, should remove 
  /// <summary>
  ///   If Collider exists, setup & override it as provided params.
  /// </summary>
  public static T AddAndSetupColliderIfNotExist<T>(this GameObject go, bool isTrigger = true)
    where T : Collider {
    var collider = go.GetComponent<T>() ?? go.AddComponent<T>();
    collider.isTrigger = isTrigger;
    return collider;
  }

  // ? If a MonoBehaviour method is used frequently, consider move it into MonoBehaviourBase

  // ? Naming convention / API design: "perform action, after a time period, revert action"
  /// <summary>
  ///   Temporarily disable the component for a given <paramref name="timePeriod"/>.
  /// </summary>
  public static void Disable(this MonoBehaviour monoBehaviour, float timePeriod) {
    monoBehaviour.enabled = false;

    // monoBehaviour.StartCoroutine(EnableCoroutine(monoBehaviour, timePeriod));

    // TIP:
    // 1: inline function
    // void EnableMonoBehaviour() => monoBehaviour.enabled = true;
    // monoBehaviour.StartCoroutine(DelayCoroutine(timePeriod, EnableMonoBehaviour));

    // 2: lambda expression
    monoBehaviour.StartCoroutine(DelayCoroutine(timePeriod, () => monoBehaviour.enabled = true));

    // 3: delegate (anonymous function)
    // monoBehaviour.StartCoroutine(DelayCoroutine(timePeriod, delegate { monoBehaviour.enabled = true; }));
  }

  /// <summary>
  ///   Temporarily disable the components for a given <paramref name="timePeriod"/>.
  /// </summary>
  public static void Disable(this IEnumerable<MonoBehaviour> monoBehaviours, float timePeriod) {
    foreach (var monoBehaviour in monoBehaviours) monoBehaviour.Disable(timePeriod);
  }

  private static IEnumerator EnableCoroutine(Behaviour behaviour, float delay) {
    yield return new WaitForSeconds(delay);
    behaviour.enabled = true;
  }

  private static IEnumerator DelayCoroutine(Action actionBefore, float delay, Action actionAfter) {
    actionBefore?.Invoke();
    yield return new WaitForSeconds(delay);
    actionAfter?.Invoke();
  }

  private static IEnumerator DelayCoroutine(float delay, Action action) => DelayCoroutine(null, delay, action);
}