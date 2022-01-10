using UnityEngine;
using D = Drawing;

// TIP: Wrapper existing static class with same name class in different namespace to add functionalities
// ? Shoud class has the same name
namespace Enginoobz.Utils {
  public static class Draw {
    public static void Arrow(Vector3 from, Vector3 to) {
      D.Draw.Arrow(from, to);
    }
  }
}