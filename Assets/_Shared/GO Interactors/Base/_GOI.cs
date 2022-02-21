// TIP: "Menu" file to review relationships in complex generic class hierarchy 

using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract partial class GOI : MonoBehaviourSingleton<GOI> { }


public abstract partial class GOI<TSelf> : GOI
  where TSelf : GOI<TSelf> { }


/// <summary>
///   TComponent: GOInteractor applies interacting effect by adding this component type to the GO. <br />
/// </summary>
public abstract partial class GOI<TSelf, TComponent>
  : GOI<TSelf>
  where TSelf : GOI<TSelf, TComponent>
  where TComponent : MonoBehaviour { }


/// <summary>
///   Cache last effect (component) to compare with current when re-interacting.
/// </summary>
public abstract partial class GOI<TSelf, TComponent, TEffect>
  : GOI<TSelf, TComponent>
  where TSelf : GOI<TSelf, TComponent, TEffect>
  where TComponent : MonoBehaviour { }


/// <summary>
///   TComponent: GOInteractor applies interacting effect by adding this component type to the GO. <br />
///   TEffect: Effect variation when the GO is interacted. <br />
///   TCache: Cached Object/effect of the interated GO for implementing revert method.<br />
/// </summary>
public abstract partial class GOI<TSelf, TComponent, TEffect, TCache>
  : GOI<TSelf, TComponent, TEffect>
  where TSelf : GOI<TSelf, TComponent, TEffect, TCache>
  where TComponent : MonoBehaviour
  where TEffect : class // TODO: Generalize interacting effect config (enum/prefab/SO/struct)
  where TCache : class { } // TODO: Generalize - GOInteractCache


/// <summary>
///   TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br />
///   TComponent is also the interacting effect which can be setup with prefabs.
/// </summary>
public abstract partial class GOI_ComponentIsEffect<TSelf, TComponent>
  : GOI<TSelf, TComponent, TComponent>
  where TSelf : GOI_ComponentIsEffect<TSelf, TComponent>
  where TComponent : MonoBehaviour { }


public abstract class GOI_ComponentIsEffect<TSelf, TComponent, TCache>
  : GOI<TSelf, TComponent, TComponent, TCache>
  where TSelf : GOI_ComponentIsEffect<TSelf, TComponent, TCache>
  where TComponent : MonoBehaviour
  where TCache : class { }


public abstract partial class GOI_ComponentIsEffect_CacheEffect<TSelf, TComponent>
  : GOI_ComponentIsEffect<TSelf, TComponent>
  where TSelf : GOI_ComponentIsEffect_CacheEffect<TSelf, TComponent>
  where TComponent : MonoBehaviour { }


/// <summary>
///   TComponent: GOInteractor applies interacting effect by adding this component to the GO. <br />
///   TEffectEnum: Effect variation (enum) when the GO is interacted. <br />
///   TCacheObject: Cached Object of the interated GO for implementing revert method. <br />
/// </summary>
public abstract partial class GOI_EffectIsEnum<TSelf, TComponent, TEffectEnum, TCache>
  : GOI<TSelf, TComponent, GOIEffect<TEffectEnum>, TCache>
  where TSelf : GOI_EffectIsEnum<TSelf, TComponent, TEffectEnum, TCache>
  where TComponent : MonoBehaviour
  where TEffectEnum : Enum
  where TCache : Object { }