using UnityEngine;

public class TestUnionType : MonoBehaviour {
  public UnionType<string, Vector3> _hi;

  private void Start() {
    Debug.Log(_hi.Value);
  }

  private void Update() {
  }
}