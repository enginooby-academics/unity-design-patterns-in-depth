using UnityEngine;

public static class MeshUtils {
  /// <summary>
  ///   ! This can change primitive mesh permanently. Use WithScale() to get a new Mesh w/o affecting primitive mesh.
  /// </summary>
  public static void Scale(this Mesh mesh, float factor) {
    var baseVertices = mesh.vertices;
    var vertices = new Vector3[baseVertices.Length];

    for (var i = 0; i < vertices.Length; i++) {
      var vertex = baseVertices[i];
      vertex.x = vertex.x * factor;
      vertex.y = vertex.y * factor;
      vertex.z = vertex.z * factor;
      vertices[i] = vertex;
    }

    mesh.vertices = vertices;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
  }

  /// <summary>
  ///   Return a copied mesh with the specified scale in the specified axis from the original mesh.
  /// </summary>
  public static Mesh WithScale(this Mesh mesh, float factor, AxisFlag axis = AxisFlag.All) {
    var newMesh = Object.Instantiate(mesh);
    var baseVertices = mesh.vertices;
    var vertices = new Vector3[baseVertices.Length];

    for (var i = 0; i < vertices.Length; i++) {
      var vertex = baseVertices[i];
      if (axis.HasFlag(AxisFlag.X)) vertex.x = vertex.x * factor;
      if (axis.HasFlag(AxisFlag.Y)) vertex.y = vertex.y * factor;
      if (axis.HasFlag(AxisFlag.Z)) vertex.z = vertex.z * factor;
      vertices[i] = vertex;
    }

    newMesh.vertices = vertices;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    return newMesh;
  }

  // / <summary>
  // / Copy mesh by reflection.
  // / </summary>
  // public static Mesh Copy(this Mesh mesh) {
  //   var copy = new Mesh();
  //   foreach (var property in typeof(Mesh).GetProperties()) {
  //     if (property.GetSetMethod() != null && property.GetGetMethod() != null) {
  //       property.SetValue(copy, property.GetValue(mesh, null), null);
  //     }
  //   }
  //   return copy;
  // }
}