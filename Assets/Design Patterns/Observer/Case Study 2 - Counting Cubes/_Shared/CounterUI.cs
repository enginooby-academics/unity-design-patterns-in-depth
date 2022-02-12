using TMPro;
using UnityEngine;

namespace ObserverPattern.Case2 {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CounterUI : MonoBehaviour {
    private TextMeshProUGUI _label;

    private void Awake() => _label = GetComponent<TextMeshProUGUI>();

    public void SetText(int count) => _label.text = count.ToString();
  }
}