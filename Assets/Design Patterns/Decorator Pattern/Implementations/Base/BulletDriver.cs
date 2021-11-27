using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Decorator.Base {
  /// <summary>
  /// Bullet consumer where clients instantiate & make use of bullet decorators.
  /// </summary>
  public class BulletDriver : MonoBehaviour {
    [SerializeField, Min(0)]
    private int _fireDecorators;

    [SerializeField]
    private List<ForceDecoratorConfig> _forceDecorators = new List<ForceDecoratorConfig>();

    [SerializeField]
    private List<SpeedDecoratorConfig> _speedDecorators = new List<SpeedDecoratorConfig>();

    private IBullet _bullet;
    private AttackTarget _target;


    void Start() {
      _target = FindObjectOfType<AttackTarget>();

      _bullet = new Bullet();

      for (int i = 0; i < _fireDecorators; i++) {
        _bullet = new BulletFireDecorator(_bullet);
      }

      _forceDecorators.ForEach(forceDecorator => {
        _bullet = new BulletForceDecorator(_bullet, forceDecorator.bonusDamage);
      });

      _speedDecorators.ForEach(speedDecorator => {
        _bullet = new BulletSpeedDecorator(_bullet, speedDecorator.bonusSpeed);
      });
    }

    private void Update() {
      transform.LookAtAndMoveY(_target.transform.GetColliderCenter(), _bullet.Speed);
    }

    private void OnTriggerEnter(Collider other) {
      if (other.TryGetComponent<AttackTarget>(out var hitTarget)) {
        _bullet.Hit(hitTarget);
        Destroy(gameObject);
      }
    }
  }

  [Serializable, InlineProperty]
  class ForceDecoratorConfig {
    public float bonusDamage;
  }

  [Serializable, InlineProperty]
  class SpeedDecoratorConfig {
    public float bonusSpeed;
  }
}
