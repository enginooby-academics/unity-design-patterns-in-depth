using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

// ! Cannot add multiple CapsLock components in a scene due to singleton 
// -> Separate CapsLockSingleton (IsOn) and CapsLock (components, events)
public class CapsLock : MonoBehaviourSingleton<CapsLock> {
  [SerializeField] private List<MonoBehaviour> _enableComponents = new();
  [SerializeField] private List<MonoBehaviour> _disableComponents = new();
  [SerializeField] private UnityEvent<bool> _onToggle = new();

  public Action<bool> OnToggle;

  public bool IsOn { get; private set; }

  private void Start() => IsOn = ((ushort) GetKeyState(0x14) & 0xffff) != 0;

  private void Update() {
    if (Input.GetKeyDown(KeyCode.CapsLock)) Toggle();
  }

  [DllImport("user32.dll")]
  public static extern short GetKeyState(int keyCode);

  public void Toggle() {
    IsOn = !IsOn;
    OnToggle?.Invoke(IsOn);
    _onToggle?.Invoke(IsOn);
    _enableComponents.ForEach(component => component.enabled = IsOn);
    _disableComponents.ForEach(component => component.enabled = !IsOn);
  }
}