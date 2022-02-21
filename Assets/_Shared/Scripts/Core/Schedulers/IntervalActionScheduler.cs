using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

// TODO: Make InputActionScheduler class from Active mode of Spawner
// ? Rename to ActionTrigger/Invoker
public abstract class ActionScheduler : SerializableBase {
  [SerializeField] protected int _actionLimit;

  protected MonoBehaviour _actionOwner;

  [SerializeField] protected bool _enabled = false;

  [SerializeField] private UnityEvent _onActionInvoke = new(); // ? Necessery when already have Action

  protected bool _started;
  public Action Action;
  public Action OnActionInvoke;

  public abstract void Init(MonoBehaviour actionOwner, Action action);
  public abstract void Enable();
  public abstract void Disable();
}

[Serializable]
[InlineProperty]
/// <summary>
/// Repeat action by interval. Call Init() to start the timer (if enable in Inspector).
/// </summary>
public class IntervalActionScheduler : SerializableBase {
  // [PropertySpace(SpaceBefore = SECTION_SPACE)]
  [HideLabel] [DisplayAsString(false)] [ShowInInspector]
  private const string AUTO_SPAWNING_SPACE = "";

  [TabGroup("Spawning Mode", "Auto Spawning")] [SerializeField]
  private bool _enabled;

  [TabGroup("Spawning Mode", "Auto Spawning")]
  [SuffixLabel("(seconds)", Overlay = true)]
  [Min(0f)]
  [ShowIf(nameof(_enabled))]
  [SerializeField]
  private float _initialDelay = 1f;

  [TabGroup("Spawning Mode", "Auto Spawning")]
  [SuffixLabel("(seconds)", Overlay = true)]
  [ShowIf(nameof(_enabled))]
  [SerializeField]
  [MinMaxSlider(0, 100, true)]
  private Vector2 _rateRange = Vector2.one;

  private Action _action;

  // TODO: rate acceleration & max rates
  // UTIL: make gerenric serializable/SO (AccelarableRange) class for random-in-range value + acceleration + max value

  private MonoBehaviour _actionOwner;
  private bool _started;

  // TODO: Create dedicated CoroutineRunner Singleton as fallback
  public void Init(MonoBehaviour actionOwner, Action action) {
    _actionOwner = actionOwner;
    _action = action;
    if (_enabled) _actionOwner.StartCoroutine(StartEnumerator());
  }

  private IEnumerator StartEnumerator() {
    if (!_started) {
      _started = true;
      yield return new WaitForSeconds(_initialDelay);
    }

    _action?.Invoke();
    yield return new WaitForSeconds(_rateRange.Random());
    if (_enabled) _actionOwner.StartCoroutine(StartEnumerator());
  }

  public void Enable() {
    _enabled = true;
    _actionOwner.StartCoroutine(StartEnumerator());
  }

  public void Disable() {
    _enabled = false;
  }
}