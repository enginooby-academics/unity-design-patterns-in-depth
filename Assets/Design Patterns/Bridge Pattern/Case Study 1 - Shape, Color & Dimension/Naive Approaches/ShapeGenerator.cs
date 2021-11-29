using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BridgePattern.Case1.Naive {
  public class ShapeGenerator : MonoBehaviour {
    // ! Reflection sometimes causes editor crash
    [ValueDropdown(nameof(_shapeTypes)), SerializeField, SerializeReference]
    private Type _shapeType;

    [SerializeReference]
    private List<Type> _shapeTypes = new List<Type>();

    [Button]
    public void GenerateShape() {
      IShape shape = Activator.CreateInstance(_shapeType) as IShape;
      shape.Draw();
    }

    private void Reset() {
      RetrieveShapeTypes();
    }

    private void RetrieveShapeTypes() {
      _shapeTypes = TypeUtils.GetTypesOf<IShape>();
    }
  }
}
