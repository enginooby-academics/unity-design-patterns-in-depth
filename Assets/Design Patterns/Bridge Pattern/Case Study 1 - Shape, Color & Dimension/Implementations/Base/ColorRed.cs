using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgePattern.Case1.Base {
  public class ColorRed : IColor {
    public void MakeColor(GameObject gameObject) {
      gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
    }
  }
}
