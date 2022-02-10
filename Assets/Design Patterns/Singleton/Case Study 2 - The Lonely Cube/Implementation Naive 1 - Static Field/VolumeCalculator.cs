using Shared = SingletonPattern.Case2;

namespace SingletonPattern.Case2.Naive1 {
public class VolumeCalculator : Shared.VolumeCalculator {
  protected override float GetCubeSize() => Cube.StaticSize;
}
}
