using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// Client.
  /// </summary>
  public class ShapeGenerator : MonoBehaviour {
    // ! Reflection sometimes causes editor crash
    [ValueDropdown(nameof(_shapeTypes)), SerializeField, SerializeReference]
    private Type _shapeType;

    [ValueDropdown(nameof(_shapeColors)), SerializeField, SerializeReference]
    private IColor _shapeColor;

    [ValueDropdown(nameof(_shapeDimensions)), SerializeField, SerializeReference]
    private IDimension _shapeDimension;

    [SerializeReference]
    private List<Type> _shapeTypes = new List<Type>();

    [SerializeReference]
    private List<IColor> _shapeColors = new List<IColor>();

    [SerializeReference]
    private List<IDimension> _shapeDimensions = new List<IDimension>();

    [Button]
    public void GenerateShape() {
      Shape shape = Activator.CreateInstance(_shapeType) as Shape;
      shape.SetColor(_shapeColor);
      shape.SetDimension(_shapeDimension);
      shape.Draw();
    }

    private void Reset() {
      RetrieveShapesColorsAndDimensions();
    }

    private void RetrieveShapesColorsAndDimensions() {
      _shapeTypes = TypeUtils.GetTypesOf<Shape>();
      _shapeColors = TypeUtils.GetInstancesOf<IColor>();
      _shapeDimensions = TypeUtils.GetInstancesOf<IDimension>();
    }
  }
}
