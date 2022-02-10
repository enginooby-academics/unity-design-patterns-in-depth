using UnityEngine;
using TMPro;

namespace ObserverPattern.Case2 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : MonoBehaviour {
    private TextMeshProUGUI _label;

    void Awake() => _label = GetComponent<TextMeshProUGUI>();

    public void SetText(int count) => _label.text = count.ToString();
  }
}
