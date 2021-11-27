using UnityEngine;

namespace Decorator.Base {
  public class Gun : MonoBehaviour {
    [SerializeField]
    private GameObject _bulletPrefab;

    public void Shoot() {
      Instantiate(_bulletPrefab);
      // IBullet bullet = new GameObject("Bullet").AddComponent<Bullet>();
    }

    void Start() {

    }

    void Update() {
      if (MouseButton.Left.IsDown()) Shoot();
    }
  }
}
