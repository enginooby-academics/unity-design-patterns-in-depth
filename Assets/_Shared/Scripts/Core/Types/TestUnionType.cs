using UnityEngine;

public class TestUnionType : MonoBehaviour {
  public UnionType<string, Vector3> _hi;

  void Start() {
    Debug.Log(_hi.Value);
  }

  void Update() {

  }
}
