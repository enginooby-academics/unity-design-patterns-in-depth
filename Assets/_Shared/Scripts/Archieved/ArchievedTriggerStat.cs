/// Usage: setup areas to trigger any stat of any Singleton (esp. Game Manager)


using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

public abstract class ArchievedTriggerStat<T> : ArchievedTrigger {
  [PropertyOrder(-1)]
  // TIP: use this attribute for abbreviation names (e.g. VFX w/o this displayed as V F X)    
  [LabelText("$StatLabel")] // FIX: string reference
  [SerializeField]
  public T statValue;

  public abstract string StatLabel { get; }


  public override void InvokeActionImpl() {
    UpdateStat();
  }

  public abstract void UpdateStat();
}