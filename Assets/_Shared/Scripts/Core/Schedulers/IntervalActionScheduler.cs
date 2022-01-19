using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

// TODO: Make InputActionScheduler class from Active mode of Spawner
// ? Rename to ActionTrigger/Invoker
public abstract class ActionScheduler : SerializableBase {
  [SerializeField]
  protected bool _enabled = false;
  [SerializeField]
  protected int _actionLimit;
  [SerializeField]
  private UnityEvent _onActionInvoke = new UnityEvent(); // ? Necessery when already have Action

  protected MonoBehaviour _actionOwner;
  public Action Action;
  public Action OnActionInvoke;
  protected bool _started;

  public abstract void Init(MonoBehaviour actionOwner, Action action);
  public abstract void Enable();
  public abstract void Disable();
}

[Serializable, InlineProperty]
/// <summary>
/// Repeat action by interval. Call Init() to start the timer (if enable in Inspector).
/// </summary>
public class IntervalActionScheduler : SerializableBase {
  // [PropertySpace(SpaceBefore = SECTION_SPACE)]
  [HideLabel, DisplayAsString(false), ShowInInspector]
  const string AUTO_SPAWNING_SPACE = "";

  [TabGroup("Spawning Mode", "Auto Spawning")]
  [SerializeField]
  private bool _enabled = false;

  [TabGroup("Spawning Mode", "Auto Spawning")]
  [SuffixLabel("(seconds)", Overlay = true), Min(0f)]
  [ShowIf(nameof(_enabled))]
  [SerializeField]
  private float _initialDelay = 1f;

  [TabGroup("Spawning Mode", "Auto Spawning")]
  [SuffixLabel("(seconds)", Overlay = true)]
  [ShowIf(nameof(_enabled))]
  [SerializeField]
  [MinMaxSlider(0, 100, true)]
  private Vector2 _rateRange = Vector2.one;

  // TODO: rate acceleration & max rates
  // UTIL: make gerenric serializable/SO (AccelarableRange) class for random-in-range value + acceleration + max value

  private MonoBehaviour _actionOwner;
  private Action _action;
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