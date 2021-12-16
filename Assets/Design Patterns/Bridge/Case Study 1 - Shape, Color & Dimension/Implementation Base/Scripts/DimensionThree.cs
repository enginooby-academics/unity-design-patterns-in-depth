using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgePattern.Case1.Base {
  public class DimensionThree : IDimension {
    public void MakeDimension(GameObject gameObject) {
      gameObject.transform.localScale = Vector3.one;
    }
  }
}
