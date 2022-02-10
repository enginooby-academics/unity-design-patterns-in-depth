using UnityEngine;

namespace Prototype {
  public interface ICloneable {
    object Clone(Vector3? newPos);
  }
}