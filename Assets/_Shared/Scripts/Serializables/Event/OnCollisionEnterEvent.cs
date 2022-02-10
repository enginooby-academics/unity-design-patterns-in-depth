#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

using System;

namespace Enginoobz {

  [Serializable, InlineProperty]
  public class OnCollisionEnterEvent : ColliderEvent { }
}