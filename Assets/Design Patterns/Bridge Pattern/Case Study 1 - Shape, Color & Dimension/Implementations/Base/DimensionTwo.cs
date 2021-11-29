using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgePattern.Case1.Base {
  public class DimensionTwo : IDimension {
    public void MakeDimension(GameObject gameObject) {
      gameObject.transform.localScale = new Vector3(1, 1, .01f);
    }
  }
}
