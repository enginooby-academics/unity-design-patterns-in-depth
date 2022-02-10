#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System;
using UnityEngine;
using static UnityEngine.GameObject;

namespace BridgePattern.Case1.Naive3 {
  public class ShapeGenerator : MonoBehaviour {
    public enum Shape { Cube, Sphere }
    public enum Size { Big, Small }
    public enum Skin { Light, Dark }

    [SerializeField, EnumToggleButtons]
    private Shape _currentShape;

    [SerializeField, EnumToggleButtons]
    private Size _currentSize;

    [SerializeField, EnumToggleButtons]
    private Skin _currentSkin;


    [Button]
    public void GenerateShape() {
      GameObject go = _currentShape switch
      {
        Shape.Cube => CreatePrimitive(PrimitiveType.Cube),
        Shape.Sphere => CreatePrimitive(PrimitiveType.Sphere),
        _ => throw new ArgumentOutOfRangeException(),
      };

      switch (_currentSize) {
        case Size.Big:
          go.SetScale(UnityEngine.Random.Range(2f, 3f));
          break;
        case Size.Small:
          go.SetScale(UnityEngine.Random.Range(.5f, 1f));
          break;
      }

      switch (_currentSkin) {
        case Skin.Light:
          go.SetMaterialColor(Color.white);
          break;
        case Skin.Dark:
          go.SetMaterialColor(Color.black);
          break;
      }
    }
  }
}
