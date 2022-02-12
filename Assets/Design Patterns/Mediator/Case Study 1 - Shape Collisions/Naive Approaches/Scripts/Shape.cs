using System;
using System.Collections.Generic;
using UnityEngine;

namespace MediatorPattern.Case1.Naive {
  public abstract class Shape : MonoBehaviour {
    protected List<Type> _collidableShapes = new List<Type>();

    private void OnTriggerEnter(Collider other) {
      if (other.TryGetComponent(typeof(Shape), out var otherShape))
        if (_collidableShapes.Contains(otherShape.GetType()))
          gameObject.InvertTranslationalDirection();
    }
  }
}