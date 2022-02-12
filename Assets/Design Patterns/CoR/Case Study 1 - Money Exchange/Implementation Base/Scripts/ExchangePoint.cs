using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using Enginoobz.Attribute;
#endif

namespace CoRPattern.Case1.Base {
  public class ExchangePoint : MonoBehaviour {
    [SerializeField] [Min(1)] private int _currency;
    [SerializeField] private ExchangePoint _nextPoint; // TODO: Draw line to next point

    [InlineEditor] [SerializeField] private GameObject _currencyPrefab;

    [Button]
    public void Exchange(GameObject moneyBag, int amount) {
      var numOfCurrencies = amount / _currency;
      var remainder = amount % _currency;
      if (numOfCurrencies > 0) Debug.Log($"[{_currency}] - {numOfCurrencies}");

      for (var i = 0; i < numOfCurrencies; i++) {
        var currency = Instantiate(_currencyPrefab, moneyBag.transform);
        currency.transform.position = moneyBag.transform.position; // + random
      }

      // Debug.Log("Remaining: " + remainder);
      if (_nextPoint) // moneyBag.MoveTo(_nextPoint);
        _nextPoint.Exchange(moneyBag, remainder);
      else
        Debug.Log("Final remaining: " + remainder);
    }
  }
}