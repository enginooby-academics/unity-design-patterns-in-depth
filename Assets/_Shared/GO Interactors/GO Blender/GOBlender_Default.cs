// using UnityEngine;


// /// <summary>
// /// * Use cases: colorize GOs, see through, un-highlight
// /// </summary>
// public abstract class GOBlender<TSelf, TComponent, TEffectEnum, TCache> : GOI_EffectIsEnum<TSelf, TComponent, TEffectEnum, TCache>
// where TSelf : GOBlender<TSelf, TComponent, TEffectEnum, TCache>
// where TComponent : UnityEngine.MonoBehaviour
// where TEffectEnum : struct
// where TCache : Object {
// }

// public class GOBlender : GOBlender<GOBlender, Follower, MouseButton, Transform> {
//   public override void Interact(GameObject go, MouseButton effect) {
//     print("GOBlender Interact");
//   }

//   protected override Transform CacheObject(GameObject go) {
//     return null;
//   }
// }

// public abstract class GOBlenderEffectEnum<TComponent, TEffectEnum, TCache> : GOBlender<GOBlenderEffectEnum<TComponent, TEffectEnum, TCache>, TComponent, TEffectEnum, TCache>
// where TComponent : UnityEngine.MonoBehaviour
// where TEffectEnum : struct
// where TCache : Object {
//   // public static new TSelf Instance => MonoBehaviourSingleton<TSelf>.Instance;
//   // protected override bool DoesInstanceExist => _instance && _instance.GetType() == typeof(TSelf);
// }

// // public abstract class GOBlender<TSelf> : GOBlender
// // where TSelf : GOBlender<TSelf> {
// //   public static new TSelf Instance => MonoBehaviourSingleton<TSelf>.Instance;
// //   protected override bool DoesInstanceExist => _instance && _instance.GetType() == typeof(TSelf);
// // }