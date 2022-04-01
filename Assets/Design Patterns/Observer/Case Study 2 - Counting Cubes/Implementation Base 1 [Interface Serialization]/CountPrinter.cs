using Enginooby.Attribute;
using UnityEngine;

namespace ObserverPattern.Case2.Base1 {
  public class CountPrinter : MonoBehaviour, ICountObserver {
    private void Start() => FindObjectOfType<Counter>().AddCountObserver(this);

    public void OnCountChanged(int count) => print(count);

    [Button]
    private void Unsubscribe() {
      FindObjectOfType<Counter>().RemoveCountObserver(this);
    }
  }
}