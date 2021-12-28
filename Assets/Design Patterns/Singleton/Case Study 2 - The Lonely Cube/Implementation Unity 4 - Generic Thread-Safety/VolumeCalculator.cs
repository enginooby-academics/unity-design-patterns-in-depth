using Shared = SingletonPattern.Case2;

namespace SingletonPattern.Case2.Unity4 {
  public class VolumeCalculator : Shared.VolumeCalculator {
    protected override float GetCubeSize() => Cube.Instance.Value.Size; // ! return 0
  }
}
