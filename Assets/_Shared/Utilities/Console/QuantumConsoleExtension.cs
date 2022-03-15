#if ASSET_QUANTUM_CONSOLE
using System.Collections.Generic;
using System.Linq;
using QFSW.QC;
using UnityEngine;

[RequireComponent(typeof(QuantumConsole))]
public class QuantumConsoleExtension : MonoBehaviour {
  [Tooltip("Usually controllers should be disable while typing in the console.")]
  [SerializeField] private List<MonoBehaviour> _disableOnConsoleActive = new();

  private QuantumConsole _quantumConsole;

  private void OnEnable() {
    _quantumConsole.OnActivate += DeactivateComponents;
    _quantumConsole.OnDeactivate += ActivateComponents;
  }

  private void OnDisable() {
    _quantumConsole.OnActivate -= DeactivateComponents;
    _quantumConsole.OnDeactivate -= ActivateComponents;
  } 

  private void Awake() {
    _quantumConsole = GetComponent<QuantumConsole>();
#if ULTIMATE_CHARACTER_CONTROLLER_UNIVERSALRP // Replace by appropriate symbol 
    _disableOnConsoleActive.AddRange(
      FindObjectsOfType<Opsive.UltimateCharacterController.Character.UltimateCharacterLocomotionHandler>());
#endif

#if ASSET_GAME_CREATOR
    _disableOnConsoleActive.AddRange(FindObjectsOfType<GameCreator.Runtime.Characters.Character>());
#endif
    
    // TODO: Add EventSystem in scene if not available
  }

  private void ActivateComponents() => SetActive(true);

  private void DeactivateComponents() => SetActive(false);

  private void SetActive(bool isActive) {
    foreach (var monoBehaviour in _disableOnConsoleActive.Where(monoBehaviour => monoBehaviour != null)) {
      monoBehaviour.enabled = isActive;
    }
  }
}
#endif