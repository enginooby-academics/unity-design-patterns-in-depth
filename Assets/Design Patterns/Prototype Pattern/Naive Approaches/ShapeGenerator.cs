using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorUtils;
using static QuaternionUtils;

namespace Prototype.Naive {
  public class ShapeGenerator : MonoBehaviourSingleton<ShapeGenerator> {
    public ProceduralShape template;

    void Start() {
      var cube = new ProceduralCube("Cube", Color.green, v0, q0, v1);
    }

    public void SpawnTemplate() {

    }

    public void SpawnRandom() {

    }

    void Update() {

    }
  }
}