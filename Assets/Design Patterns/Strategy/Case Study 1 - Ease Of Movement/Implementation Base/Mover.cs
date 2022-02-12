#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System.Collections.Generic;
using UnityEngine;
using static RayUtils;

namespace Strategy.Base {
  public class Mover : MonoBehaviour {
    [SerializeField] private float _speed = 5f;

    [SerializeReference] private List<IMovementEase> _movementEases = new List<IMovementEase>();

    [ValueDropdown(nameof(_movementEases))] [SerializeField] [SerializeReference]
    private IMovementEase _movementEase;

    private void Reset() {
      RetrieveMovementEases();
    }

    private void Update() {
      HandleMovement();
    }

    [Button]
    /// <summary>
    /// Update all types of movement ease if adding new type.
    /// </summary>
    public void RetrieveMovementEases() {
      _movementEases = TypeUtils.GetInstancesOf<IMovementEase>();
      if (_movementEases.IsSet()) _movementEase = _movementEases[0];
    }

    private void HandleMovement() {
      if (MouseButton.Left.IsDown()) {
        var mousePosOnGround = MousePosOnRayHit.Value.WithY(0);
        _movementEase.Move(gameObject, mousePosOnGround, _speed);
      }
    }
  }
}