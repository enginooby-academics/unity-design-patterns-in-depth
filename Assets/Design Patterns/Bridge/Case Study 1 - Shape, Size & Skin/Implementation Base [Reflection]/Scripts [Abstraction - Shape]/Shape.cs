using UnityEngine;

namespace BridgePattern.Case1.Base {
  /// <summary>
  /// * [The 'Abstraction' class]
  /// </summary>
  public abstract class Shape {
    protected ISize _size;
    protected ISkin _skin;

    public void SetSize(ISize size) => _size = size;
    public void SetSkin(ISkin skin) => _skin = skin;

    public void Display() {
      GameObject go = CreateGameObject();
      _size?.ProcessSize(go);
      _skin?.ProcessSkin(go);
    }

    protected abstract GameObject CreateGameObject();
  }
}
