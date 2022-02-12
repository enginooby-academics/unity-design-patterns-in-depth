using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

public class AreaPointDataTest : MonoBehaviourBase {
  [SerializeField] [InlineEditor] private AreaPointData _data;

  [SerializeField] private float _scale = 1f;

  // TODO:
  // + Color
  // + Radius
  // + Link
  // + Rotate
  // + Offset

  private void OnDrawGizmos() {
    _data?.DrawGizmos(transform.position, _scale);
  }
}

[Serializable]
public class AreaPointModifier {
  [SerializeField] private float _scale = 1f;
}