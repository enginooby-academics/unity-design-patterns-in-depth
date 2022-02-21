using System;

namespace Enginooby.Utils {
  /// <summary>
  ///   Helper to create instance of generic tuple types.
  /// </summary>
  public static class Tuple {
    public static Tuple<T1> Create<T1>(T1 item1) => new(item1);

    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) => new(item1, item2);

    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) => new(item1, item2, item3);

    public static void Demo() {
      var tuple1A = new Tuple<int>(6);
      Tuple<int> tuple1B = new(6);
      var tuple1C = Create(3);

      var tuple3A = new Tuple<int, string, bool>(1, "a", true);
      Tuple<int, string, bool> tuple3B = new(1, "a", true);
      var tuple3C = Create(1, "a", true);
    }
  }
}