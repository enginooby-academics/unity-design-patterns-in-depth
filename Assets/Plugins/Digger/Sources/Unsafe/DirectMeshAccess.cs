using System;
using UnityEngine;

public static unsafe class DirectMeshAccess
{
    public static readonly Vector3[] VertexArray = new Vector3[65532];
    public static readonly Vector3[] NormalArray = new Vector3[65532];
    public static readonly int[] TriangleArray = new int[65532];
    public static readonly Color[] ColorArray = new Color[65532];
    public static readonly Vector2[] Uv0Array = new Vector2[65532];
    public static readonly uint[] InfoArray = new uint[65532];

    public static void DirectSet(Mesh mesh, int vertexCount, int triangleCount)
    {
        fixed (void* ptr = VertexArray) {
            *((UIntPtr*) ptr - 1) = (UIntPtr) (vertexCount);
            mesh.vertices = VertexArray;
            *((UIntPtr*) ptr - 1) = (UIntPtr) (65532);
        }

        fixed (void* ptr = NormalArray) {
            *((UIntPtr*) ptr - 1) = (UIntPtr) (vertexCount);
            mesh.normals = NormalArray;
            *((UIntPtr*) ptr - 1) = (UIntPtr) (65532);
        }

        fixed (void* ptr = TriangleArray) {
            *((UIntPtr*) ptr - 1) = (UIntPtr) (triangleCount);
            mesh.triangles = TriangleArray;
            *((UIntPtr*) ptr - 1) = (UIntPtr) (65532);
        }
    }

    public static void DirectSetTotal(Mesh mesh, int vertexCount, int triangleCount)
    {
        DirectSet(mesh, vertexCount, triangleCount);

        fixed (void* ptr = ColorArray) {
            *((UIntPtr*) ptr - 1) = (UIntPtr) (vertexCount);
            mesh.colors = ColorArray;
            *((UIntPtr*) ptr - 1) = (UIntPtr) (65532);
        }

        fixed (void* ptr = Uv0Array) {
            *((UIntPtr*) ptr - 1) = (UIntPtr) (vertexCount);
            mesh.uv = Uv0Array;
            *((UIntPtr*) ptr - 1) = (UIntPtr) (65532);
        }
    }
}