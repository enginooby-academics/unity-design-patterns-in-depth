using System.Collections.Generic;
using System;
using UnityEngine;
using static RayUtils;
using Sirenix.OdinInspector;

namespace Strategy.Base {
  public class Mover : MonoBehaviour {
    [SerializeField]
    private float _speed = 5f;

    [ValueDropdown(nameof(_movementEases)), SerializeField, SerializeReference]
    private IMovementEase _movementEase;

    [SerializeReference]
    private List<IMovementEase> _movementEases = new List<IMovementEase>();

    private void Reset() {
      RetrieveMovementEases();
    }

    void Update() {
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
        Vector3 mousePosOnGround = MousePosOnRayHit.Value.WithY(0);
        _movementEase.Move(gameObject, mousePosOnGround, _speed);
      }
    }
  }
}
