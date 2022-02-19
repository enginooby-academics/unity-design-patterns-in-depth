#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RayUtils;

namespace Strategy.Base {
  public class Mover : MonoBehaviour {
    [SerializeField] private float _speed = 5f;

    [SerializeReference] private IEnumerable<IMovementEase> _movementEases; // TODO: Use ReferenceConcreteType

    [ValueDropdown(nameof(_movementEases))] [SerializeReference]
    private IMovementEase _movementEase;

    private void Reset() => RetrieveMovementEases();

    private void Update() => HandleMovement();

    /// <summary>
    /// Update all types of movement ease if adding new type.
    /// </summary>
    [Button]
    public void RetrieveMovementEases() {
      _movementEases = TypeUtils.GetInstancesOf<IMovementEase>();
      _movementEase = _movementEases.ElementAt(0);
    }

    private void HandleMovement() {
      if (MouseButton.Left.IsDown()) {
        var mousePosOnGround = MousePosOnRayHit.Value.WithY(0);
        _movementEase.Move(gameObject, mousePosOnGround, _speed);
      }
    }
  }
}