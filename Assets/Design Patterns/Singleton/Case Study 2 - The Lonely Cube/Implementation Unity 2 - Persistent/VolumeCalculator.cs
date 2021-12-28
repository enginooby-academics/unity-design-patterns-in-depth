using Shared = SingletonPattern.Case2;

namespace SingletonPattern.Case2.Unity2 {
  public class VolumeCalculator : Shared.VolumeCalculator {
    protected override float GetCubeSize() => Cube.Instance.Size;
  }
}
