using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginoobz.Attribute;
#endif

namespace Enginoobz {
  [Serializable]
  [InlineProperty]
  public class OnCollisionEnterEvent : ColliderEvent {
  }
}