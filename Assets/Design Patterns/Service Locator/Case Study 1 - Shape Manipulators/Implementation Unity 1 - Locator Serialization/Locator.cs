using UnityEngine;

namespace ServiceLocatorPattern.Case1.Unity1 {
  /// <summary>
  /// * The 'service locator'
  /// Manage all shape manipulators.
  /// </summary>
  public class Locator : MonoBehaviourSingleton<Locator> {
    [SerializeField] private ReferenceConcreteType<IShapeResizer> _shapeResizerType;
    [SerializeField] private ReferenceConcreteType<IShapeMover> _shapeMoverType;

    private IShapeResizer _shapeResizer;
    private IShapeMover _shapeMover;

    public IShapeResizer ShapeResizer => _shapeResizer ??= _shapeResizerType.CreateInstance();
    public IShapeMover ShapeMover => _shapeMover ??= _shapeMoverType.CreateInstance();
  }
}